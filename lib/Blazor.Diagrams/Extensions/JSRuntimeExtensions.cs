using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Extensions
{
    public static class JSRuntimeExtensions
    {
        public static async Task<Rectangle> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
            => await jsRuntime.InvokeAsync<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", element);

        public static IEnumerable<Rectangle> GetAllBoundingClientRect(this IJSRuntime jsRuntime, IEnumerable<ElementReference> elements)
            => ((IJSInProcessRuntime)jsRuntime).Invoke<IEnumerable<Rectangle>>("ZBlazorDiagrams.getAllBoundingClientRect", elements);

        public static async Task<IEnumerable<Rectangle>> GetAllBoundingClientRectAsync(this IJSRuntime jsRuntime, IEnumerable<ElementReference> elements)
            => await jsRuntime.InvokeAsync<IEnumerable<Rectangle>>("ZBlazorDiagrams.getAllBoundingClientRect", elements);

        public static async Task ObserveResizes<T>(this IJSRuntime jsRuntime, ElementReference element,
            DotNetObjectReference<T> reference) where T : class
        {
            await jsRuntime.InvokeVoidAsync("ZBlazorDiagrams.observe", element, reference, element.Id);
        }

        public static async Task UnobserveResizes(this IJSRuntime jsRuntime, ElementReference element)
        {
            await jsRuntime.InvokeVoidAsync("ZBlazorDiagrams.unobserve", element, element.Id);
        }
    }
}
