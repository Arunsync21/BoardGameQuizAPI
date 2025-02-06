using PdfSharp.Fonts;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

namespace BoardGameQuizAPI.Models
{
    public class CustomFontResolver : IFontResolver
    {
        private readonly string _fontPath;
        private readonly string _arialFontPath;
        private readonly string _poppinsFontPath;

        public CustomFontResolver(IWebHostEnvironment webHostEnvironment)
        {
            _fontPath = Path.Combine(webHostEnvironment.WebRootPath, "fonts", "MagnoliaScript.ttf");
            _arialFontPath = Path.Combine(webHostEnvironment.WebRootPath, "fonts", "Arial.ttf");
            _poppinsFontPath = Path.Combine(webHostEnvironment.WebRootPath, "fonts", "Poppins-Regular.ttf");

            if (!File.Exists(_arialFontPath))
                throw new FileNotFoundException($"Arial font file not found at: {_arialFontPath}");

            if (!File.Exists(_fontPath))
            {
                throw new FileNotFoundException($"Font file not found at: {_fontPath}");
            }
            if (!File.Exists(_poppinsFontPath))
            {
                throw new FileNotFoundException($"Font file not found at: {_poppinsFontPath}");
            }
        }

        public byte[] GetFont(string faceName)
        {
            if (faceName == "MagnoliaScript")
            {
                return File.ReadAllBytes(_fontPath);
            }
            if (faceName == "Arial")
                return File.ReadAllBytes(_arialFontPath);
            if (faceName == "Poppins-Regular")
                return File.ReadAllBytes(_poppinsFontPath);

            throw new InvalidOperationException($"Font '{faceName}' not found.");
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Magnolia Script", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("MagnoliaScript"); // Internal reference name
            }
            if (familyName.Equals("Poppins", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("Poppins-Regular");
            }
            if (familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
                return new FontResolverInfo("Arial");
            return null;
        }
    }
}
