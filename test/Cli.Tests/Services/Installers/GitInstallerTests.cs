using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cli.Services;
using Cli.Services.Installers;
using Cli.Services.Installers.Vcs;
using LibGit2Sharp;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Cli.Tests.Services.Installers
{
    public class GitInstallerTests
    {
        private const string WorkingDirectory = "directory";
        private const string CloneUrl = "https://example.com/repo.git";

        private readonly AutoMocker _mocker = new();
        private readonly GitInstaller _installer;

        private static readonly ServiceSource _defaultSource = new() {
            Type = SourceType.Git, CloneUrl = CloneUrl
        };

        private static readonly InstallationContext _defaultContext = new(
            WorkingDirectory,
            new ServiceEntry(),
            new[] { _defaultSource });

        public GitInstallerTests()
        {
            _installer = _mocker.CreateInstance<GitInstaller>();
        }

        [Theory]
        [InlineData("not a url")]
        [InlineData("www.example.com")]
        [InlineData("svn://192.168.420.69/svn-repo")]
        public void Constructor_RequiresValidGitUrl(string url)
        {
            var repository = _mocker.GetMock<IRepositoryFunctions>();

            Assert.Throws<ArgumentException>(() => new GitInstaller(url, repository.Object));
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.Git)]
        public void DoesNotApplyToNonGitSources(SourceType type)
        {
            var context = _defaultContext with {
                Sources = new[] { _defaultSource with { Type = type } }
            };

            var result = _installer.AppliesTo(context);

            Assert.False(result);
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStrings))]
        public void DoesNotApplyToInvalidCloneUrl(string cloneUrl)
        {
            var context = _defaultContext with {
                Sources = new[] { _defaultSource with { CloneUrl = cloneUrl } }
            };

            var result = _installer.AppliesTo(context);

            Assert.False(result);
        }

        [Fact]
        public void AppliesToValidContext()
        {
            var result = _installer.AppliesTo(_defaultContext);

            Assert.True(result);
        }

        [Fact]
        public async Task InstallAsync_ClonesRepository()
        {
            var repository = _mocker.GetMock<IRepositoryFunctions>();
            repository.Setup(x => x.IsValid(WorkingDirectory)).Returns(false);

            await _installer.InstallAsync(_defaultContext).AsTask();

            repository.Verify(x => x.Clone(CloneUrl, WorkingDirectory, It.IsAny<CloneOptions>()));
        }

        [Fact]
        public async Task InstallAsync_SkipsCloneWhenRepositoryExists()
        {
            var repository = _mocker.GetMock<IRepositoryFunctions>();
            repository.Setup(x => x.IsValid(WorkingDirectory)).Returns(true);

            await _installer.InstallAsync(_defaultContext);

            repository.Verify(
                x => x.Clone(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CloneOptions>()),
                Times.Never);
        }

        [Fact]
        public async Task InstallAsync_InstallsExplicitCloneUrl()
        {
            const string cloneUrl = "https://different.example.com/repo.git";
            var repository = _mocker.GetMock<IRepositoryFunctions>();
            var installer = new GitInstaller(cloneUrl, repository.Object);

            await installer.InstallAsync(_defaultContext);

            repository.Verify(x => x.Clone(cloneUrl, WorkingDirectory, It.IsAny<CloneOptions>()));
            repository.Verify(x => x.Clone(CloneUrl, It.IsAny<string>(), It.IsAny<CloneOptions>()), Times.Never);
        }

        [Fact]
        public async Task InstallAsync_InstallsAllSources()
        {
            const string url1 = "https://1.example.com/repo.git";
            const string url2 = "https://2.example.com/repo.git";
            var context = _defaultContext with {
                Sources = new[] {
                    _defaultSource with { CloneUrl = url1 },
                    _defaultSource with { CloneUrl = url2 },
                }
            };
            var repository = _mocker.GetMock<IRepositoryFunctions>();

            await _installer.InstallAsync(context);

            repository.Verify(x => x.Clone(url1, WorkingDirectory, It.IsAny<CloneOptions>()));
            repository.Verify(x => x.Clone(url2, WorkingDirectory, It.IsAny<CloneOptions>()));
            repository.Verify(
                x => x.Clone(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CloneOptions>()),
                Times.Exactly(2));
        }

        [Fact]
        public async Task InstallAsync_InstallsToServiceNameSubDirectory()
        {
            const string name = "serviceName";
            var expected = $"{WorkingDirectory}/{name}".ToLower();
            var context = _defaultContext with {
                Service = new ServiceEntry { Name = name }
            };
            var repository = _mocker.GetMock<IRepositoryFunctions>();

            await _installer.InstallAsync(context);
            
            repository.Verify(x => x.Clone(CloneUrl, expected, It.IsAny<CloneOptions>()));
        }

        [Theory]
        [ClassData(typeof(InvalidTypesAndUrls))]
        public async Task InvokeAsync_DoesNotInstallWhenNotApplicable(SourceType type, string? cloneUrl)
        {
            var context = _defaultContext with {
                Sources = new[] {
                    _defaultSource with {
                        Type = type,
                        CloneUrl = cloneUrl
                    }
                }
            };
            var repository = _mocker.GetMock<IRepositoryFunctions>();

            await _installer.InvokeAsync(context, _ => ValueTask.CompletedTask);
            
            repository.Verify(
                x => x.Clone(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CloneOptions>()),
                Times.Never);
        }

        [Theory]
        [ClassData(typeof(InvalidTypesAndUrls))]
        public async Task InvokeAsync_InvokesNextDelegateWhenNotApplicable(SourceType type, string? cloneUrl)
        {
            var context = _defaultContext with {
                Sources = new[] {
                    _defaultSource with {
                        Type = type,
                        CloneUrl = cloneUrl
                    }
                }
            };
            var flag = false;

            await _installer.InvokeAsync(context, _ => {
                flag = true;
                return ValueTask.CompletedTask;
            });
            
            Assert.True(flag);
        }

        private static IEnumerable<object[]> SourceTypeValuesExcept(SourceType type) => SourceTypeValues.Except(type);

        private class InvalidTypesAndUrls : TheoryData<SourceType, string?>
        {
            public InvalidTypesAndUrls()
            {
                foreach (var type in SourceTypeValuesExcept(SourceType.Git).Select(x => (SourceType)x.Single()))
                {
                    foreach (var cloneUrl in new NullOrWhitespaceStrings().Select(x => x.Single() as string))
                    {
                        Add(type, cloneUrl);
                    }
                }
            }
        }
    }
}
