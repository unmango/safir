using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Reflection;
using System.Threading.Tasks;

namespace Safir.CommandLine
{
    /// <summary>
    /// Extensions for configuring handlers on a <see cref="CommandBuilder"/>
    /// </summary>
    public static class CommandBuilderHandlerExtensions
    {
        /// <summary>
        /// Sets <paramref name="handler"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="handler">The handler to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T>(this T builder, ICommandHandler handler)
            where T : CommandBuilder
        {
            builder.Command.Handler = handler;

            return builder;
        }

        /// <summary>
        /// Sets <paramref name="delegate"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="delegate">The delegate to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T>(this T builder, Delegate @delegate)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(@delegate));
        }

        /// <summary>
        /// Sets <paramref name="methodInfo"/> as the handler for the command built by <paramref name="builder"/>
        /// with the optional target <paramref name="target"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="methodInfo">The method to use.</param>
        /// <param name="target">The target for invocation.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T>(this T builder, MethodInfo methodInfo, object? target = null)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(methodInfo, target));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T>(this T builder, Action action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1>(this T builder, Action<T1,T1> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2>(this T builder, Action<T1,T1, T2> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3>(this T builder, Action<T1,T1, T2, T3> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4>(this T builder, Action<T1,T1, T2, T3, T4> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5>(this T builder, Action<T1,T1, T2, T3, T4, T5> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6>(this T builder, Action<T1,T1, T2, T3, T4, T5, T6> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6, T7>(this T builder, Action<T1,T1, T2, T3, T4, T5, T6, T7> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T>(this T builder, Func<int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1>(this T builder, Func<T1, int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2>(this T builder, Func<T1, T2, int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3>(this T builder, Func<T1, T2, T3, int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4>(this T builder, Func<T1, T2, T3, T4, int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5>(this T builder, Func<T1, T2, T3, T4, T5, int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6>(this T builder, Func<T1, T2, T3, T4, T5, T6, int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6, T7>(this T builder, Func<T1, T2, T3, T4, T5, T6, T7, int> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T>(this T builder, Func<Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1>(this T builder, Func<T1, Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2>(this T builder, Func<T1, T2, Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3>(this T builder, Func<T1, T2, T3, Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4>(this T builder, Func<T1, T2, T3, T4, Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5>(this T builder, Func<T1, T2, T3, T4, T5, Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6>(this T builder, Func<T1, T2, T3, T4, T5, T6, Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6, T7>(this T builder, Func<T1, T2, T3, T4, T5, T6, T7, Task> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T>(this T builder, Func<Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1>(this T builder, Func<T1, Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2>(this T builder, Func<T1, T2, Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3>(this T builder, Func<T1, T2, T3, Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4>(this T builder, Func<T1, T2, T3, T4, Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5>(this T builder, Func<T1, T2, T3, T4, T5, Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6>(this T builder, Func<T1, T2, T3, T4, T5, T6, Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }

        /// <summary>
        /// Sets <paramref name="action"/> as the handler for the command built by <paramref name="builder"/>.
        /// </summary>
        /// <typeparam name="T">The type of the command builder.</typeparam>
        /// <param name="builder">The builder to configure.</param>
        /// <param name="action">The action to use.</param>
        /// <returns>The builder so calls can be chained.</returns>
        public static T UseHandler<T, T1, T2, T3, T4, T5, T6, T7>(this T builder, Func<T1, T2, T3, T4, T5, T6, T7, Task<int>> action)
            where T : CommandBuilder
        {
            return builder.UseHandler(CommandHandler.Create(action));
        }
    }
}
