using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Files.Domain;

public sealed class CropParameters
{
    public decimal X { get; set; }
    public decimal Y { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }

    public static CropParameters Create(decimal x, decimal y, decimal width, decimal height)
    { 
        if (x > 100 || x < 0)
            throw new ArgumentException("Invalid x value");
        if (y > 100 || y < 0)
            throw new ArgumentException("Invalid y value");
        if (width > 100 || width <= 0)
            throw new ArgumentException("Invalid width value");
        if (height > 100 || height <= 0)
            throw new ArgumentException("Invalid height value");

        return new()
        {
            X = x,
            Y = y,
            Width = width,
            Height = height
        };
    }
}