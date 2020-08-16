using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly Dictionary<Color, ColorCount> _colorCounts = Color.All.ToDictionary(c => c, v => new ColorCount() { Color = v, Count = 0 });
        private readonly Dictionary<Size, SizeCount> _sizeCounts = Size.All.ToDictionary(c => c, v => new SizeCount() { Size = v, Count = 0 });
        private readonly Dictionary<(Color, Size), IEnumerable<Shirt>> _shirtDictionary = new Dictionary<(Color, Size), IEnumerable<Shirt>>();
        private readonly List<Shirt> _shirtsFound = new List<Shirt>();

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;

            foreach (var color in Color.All)
            {
                foreach (var size in Size.All)
                {
                    _shirtDictionary.Add((color, size), _shirts.Where(sh => sh.Color == color && sh.Size == size));
                }
            }

        }


        public SearchResults Search(SearchOptions options)
        {
            if (options.Colors.Count == 0)
            {
                foreach (var size in options.Sizes)
                {
                    foreach (var color in Color.All)
                    {
                        UpdateCollections(color, size);
                    }
                }
            }
            else
            {
                var sizesCollection = options.Sizes.Count == 0 ? Size.All : options.Sizes;
                foreach (var color in options.Colors)
                {
                    foreach (var size in sizesCollection)
                    {
                        UpdateCollections(color, size);
                    }
                }
            }

            return new SearchResults
            {
                ColorCounts = _colorCounts.Values.ToList(),
                Shirts = _shirtsFound,
                SizeCounts = _sizeCounts.Values.ToList()
            };

        }

        private void UpdateCollections(Color color, Size size)
        {
            var tempArray = _shirtDictionary[(color, size)];
            _shirtsFound.AddRange(tempArray);
            _colorCounts[color].Count += tempArray.Count();
            _sizeCounts[size].Count += tempArray.Count();

        }
    }


}