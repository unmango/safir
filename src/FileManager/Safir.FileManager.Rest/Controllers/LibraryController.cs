using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Safir.FileManager.Domain.Repositories;
using Safir.FileManager.Messages;

namespace Safir.FileManager.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryRepository _libraries;

        public LibraryController(ILibraryRepository libraries)
        {
            _libraries = libraries;
        }

        [HttpGet]
        public async IAsyncEnumerable<ActionResult<LibraryResponse>> Get()
        {
            await foreach (var library in _libraries.GetAllAsync())
            {
                yield return Ok(new LibraryResponse { Path = library.PhysicalPath });
            }
        }
    }
}
