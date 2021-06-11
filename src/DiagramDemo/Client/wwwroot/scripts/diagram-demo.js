window.DiagramDemo = {
    getElementRectangle: function(elementRef) {
        const r = elementRef.getBoundingClientRect();
        return {
            'X': Math.round(r.x),
            'Y': Math.round(r.y),
            'Height': Math.round(r.height),
            'Width': Math.round(r.width)
        }
    }
}
