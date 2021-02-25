using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.History
{
    public class HistoryService
    {
        IJSRuntime js;
        public HistoryService(IJSRuntime jSRuntime)
        {
            js = jSRuntime;
        }
        /// <summary>
        /// Updates the most recent entry on the history stack.
        /// </summary>
        /// <typeparam name="T">The type of the state data</typeparam>
        /// <param name="stateData">The state of the data</param>
        /// <param name="pageTitle">The page title (not supported by most browsers)</param>
        /// <param name="url">The url to navigate (leave null for current url)</param>
        /// <returns></returns>
        public ValueTask ReplaceState<T>(T state, string url = null, string page_title = null)
        {
            var json = JsonSerializer.Serialize(state);
            return js.InvokeVoidAsync("window.history.replaceState", json, page_title, url);
        }
        /// <summary>
        /// Returns an <typeparamref name="T"/> type representing the state at the top of the history stack.
        /// </summary>
        /// <typeparam name="T">The type of the state data (if missing, will return null--i.e., default(T))</typeparam>
        /// <returns></returns>
        public async ValueTask<T> State<T>()
        {
            var json = await js.InvokeAsync<string>("eval", "window.history.state");
            if (string.IsNullOrEmpty(json)) return default(T);
            var state = JsonSerializer.Deserialize<T>(json);
            return state;
        }
        /// <summary>
        /// This asynchronous method goes to the previous page in session history.
        /// </summary>
        /// <returns></returns>
        public ValueTask Back() => js.InvokeVoidAsync("window.history.back");
        /// <summary>
        /// This asynchronous method goes to the next page in session history.
        /// </summary>
        /// <returns></returns>
        public ValueTask Forward() => js.InvokeVoidAsync("window.history.forward");
        /// <summary>
        /// Asynchronously loads a page from the session history, identified by its relative location to the current page.
        /// </summary>
        /// <param name="index">The index to move back or forward</param>
        /// <returns></returns>
        public ValueTask Go(int index) => js.InvokeVoidAsync("window.history.go", index);
        /// <summary>
        /// Pushes the given data onto the session history stack.
        /// </summary>
        /// <typeparam name="T">The type of the state data</typeparam>
        /// <param name="stateData">The state of the data</param>
        /// <param name="pageTitle">The page title (not supported by most browsers)</param>
        /// <param name="url">The url to navigate</param>
        /// <returns></returns>
        public ValueTask PushState<T>(T state, string url, string page_title = null)
        {
            var json = JsonSerializer.Serialize(state);
            return js.InvokeVoidAsync("window.history.pushState", json, page_title, url);
        }
        /// <summary>
        /// Allows web applications to explicitly set default scroll restoration behavior on history navigation. This property can be either auto or manual.
        /// </summary>
        /// <param name="scrollRestorationType"></param>
        /// <returns></returns>
        public ValueTask ScrollRestoration(ScrollRestorationType scrollRestorationType)
        {
            return js.InvokeVoidAsync("eval", $"window.history.scrollRestoration = '{scrollRestorationType}'");
        }
        /// <summary>
        /// Allows web applications to explicitly get default scroll restoration behavior on history navigation. This property can be either auto or manual.
        /// </summary>
        /// <param name="scrollRestorationType"></param>
        /// <returns></returns>
        public async ValueTask<ScrollRestorationType> ScrollRestoration()
        {
            Enum.TryParse<ScrollRestorationType>(await js.InvokeAsync<string>("eval", $"window.history.scrollRestoration"), ignoreCase:true, out var scroll);
            return scroll;
        }
    }
}
