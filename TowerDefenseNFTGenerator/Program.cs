using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text.Json;

public enum ProjectileTypes
{
    Dart,
    Cannonball,
    Lightning,
    Shruiken,
    Glue,
    Snow,
    Laser
}

public enum AttackOrder
{
    Closest,
    Furthest,
    LowestHealth,
    HighestHealth
}

public static class RandomExtensions
{
    public static TEnum NextEnumValue<TEnum>(this Random random)
        where TEnum : struct, Enum
    {
        TEnum[] vals = Enum.GetValues<TEnum>();
        return vals[random.Next(vals.Length)];
    }
}

public class Collection
{
    public string name = "Kitchen Tower Defense Tower";
    public string family = "Kitchen Tower Defense";
}

public class Attribute
{
    public string trait_type;
    public string value;
}

public class CreatorInfo
{
    public string address = "CB64F62SNMFqb67sXGFVQntB6NM49cyUMsHwS1hH5i4B";
    public int share = 100;
}

public class FileInfo
{
    public string uri;
    public string type = "image/png";
}

[Serializable]
public class TowerMetaData
{
    public string name;
    public string symbol;
    public string description;
    public int seller_fee_basis_points = 500;
    public string image;
    public List<Attribute> attributes;
    public Dictionary<string, List<object>> properties;
    public Collection collection = new Collection();
}

public class Program
{

    public static void DrawImage(ProjectileTypes projType, AttackOrder attOrder, int attackRange, int attackSpeed, int cost, bool detectsInvisibility, KnownColor projectTileColor, KnownColor towerColor, KnownColor backgroundColor, int i)
    {
        string nftFilePath = @"D:\NFTs\" + i + ".png";
        string origPath = "";
        switch(projType) {
            case ProjectileTypes.Dart:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseDartTower.png";
                break;
            case ProjectileTypes.Laser:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseLaserTower.png";
                break;
            case ProjectileTypes.Snow:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseSnowTower.png";
                break;
            case ProjectileTypes.Lightning:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseLightningTower.png";
                break;
            case ProjectileTypes.Glue:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseGlueTower.png";
                break;
            case ProjectileTypes.Cannonball:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseCannonballTower.png";
                break;
            case ProjectileTypes.Shruiken:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseShruikenTower.png";
                break;
            default:
                origPath = @"D:\git\TowerDefenseNFT\TowerDefenseNFTGenerator\BaseTower.png";
                break;
        }

        var bitmap = Bitmap.FromFile(origPath) as Bitmap;
        for (var x = 0; x < bitmap.Width; x++)
        {
            for (var y = 0; y < bitmap.Height; y++)
            {
                Color original = bitmap.GetPixel(x, y);
                if(original.R == 0 && original.G == 0 && original.B == 0)
                {
                    bitmap.SetPixel(x, y, Color.FromKnownColor(towerColor));
                } else if(original.R == 34 && original.G == 177 && original.B == 76)
                {
                    bitmap.SetPixel(x, y, Color.FromKnownColor(projectTileColor));
                } else if (original.R == 237 && original.G ==28 && original.B == 36 )
                {
                    bitmap.SetPixel(x, y, Color.FromKnownColor(backgroundColor));
                } else if (original.R == 255 && original.G == 255 && original.B == 255)
                {

                } else
                {
                    bitmap.SetPixel(x, y, Color.FromKnownColor(projectTileColor));
                }

            }
        }

        // Write Text
        // Create a rectangle for the entire bitmap
        RectangleF rectf = new RectangleF(0, 0, bitmap.Width, bitmap.Height);

        // Create graphic object that will draw onto the bitmap
        Graphics g = Graphics.FromImage(bitmap);

        // ------------------------------------------
        // Ensure the best possible quality rendering
        // ------------------------------------------
        // The smoothing mode specifies whether lines, curves, and the edges of filled areas use smoothing (also called antialiasing). 
        // One exception is that path gradient brushes do not obey the smoothing mode. 
        // Areas filled using a PathGradientBrush are rendered the same way (aliased) regardless of the SmoothingMode property.
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // The interpolation mode determines how intermediate values between two endpoints are calculated.
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // Use this property to specify either higher quality, slower rendering, or lower quality, faster rendering of the contents of this Graphics object.
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

        // This one is important
        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

        // Create string formatting options (used for alignment)
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Far
        };

        double backgroundContrastRatio = (Color.FromKnownColor(backgroundColor).R + Color.FromKnownColor(backgroundColor).B + Color.FromKnownColor(backgroundColor).G) / (255 * 3.0);

        if (backgroundContrastRatio >= 0.5)
        {
            // Draw the text onto the image
            g.DrawString("#" + i + " attacks " + attOrder.ToString() + " with a range of " + attackRange + ", attack speed of " + attackSpeed + ", cost of " + cost + ", and detects invisibility " + detectsInvisibility + ".", new Font("Tahoma", 10), Brushes.Black, rectf, format);
        } else
        {
            // Draw the text onto the image
            g.DrawString("#" + i + " attacks " + attOrder.ToString() + " with a range of " + attackRange + ", attack speed of " + attackSpeed + ", cost of " + cost + ", and detects invisibility " + detectsInvisibility + ".", new Font("Tahoma", 10), Brushes.White, rectf, format);
        }

        // Flush all graphics changes to the bitmap
        g.Flush();

        bitmap.Save(nftFilePath);
    }

    public static async void DoIt()
    {
        Random random = new Random();
        int towersToGenerate = 750;
        for (int i = 0; i < towersToGenerate; i++)
        {
            var projType = random.NextEnumValue<ProjectileTypes>();
            var attOrder = random.NextEnumValue<AttackOrder>();
            int attackRange = (int)Math.Round(random.NextDouble() * 50);
            int attackSpeed = (int)Math.Round(random.NextDouble() * 10);
            int cost = (int)Math.Round(random.NextDouble() * 20);
            bool detectsInvisibility = random.Next() > (Int32.MaxValue / 2);
            double contrastRatio = 0;
            KnownColor towerColor= KnownColor.White, projectileColor =KnownColor.White, backgroundColor = KnownColor.White;
            while (contrastRatio < .5)
            {
                towerColor = random.NextEnumValue<KnownColor>();
                contrastRatio = (Color.FromKnownColor(towerColor).R + Color.FromKnownColor(towerColor).B + Color.FromKnownColor(towerColor).G) / (255 * 3.0);
            }
            contrastRatio = 0;
            while (contrastRatio < .5)
            {
                projectileColor = random.NextEnumValue<KnownColor>();
                contrastRatio = (Color.FromKnownColor(projectileColor).R + Color.FromKnownColor(projectileColor).B + Color.FromKnownColor(projectileColor).G) / (255 * 3.0);
            }
            contrastRatio = .6;
            while (contrastRatio > .5)
            {
                backgroundColor = random.NextEnumValue<KnownColor>();
                contrastRatio = (Color.FromKnownColor(backgroundColor).R + Color.FromKnownColor(backgroundColor).B + Color.FromKnownColor(backgroundColor).G) / (255 * 3.0);
            }
            List<Attribute> attributes = new List<Attribute>();
            attributes.Add(new Attribute { trait_type = "AttackRange", value = attackRange.ToString() });
            attributes.Add(new Attribute { trait_type = "AttackSpeed", value = attackSpeed.ToString() });
            attributes.Add(new Attribute { trait_type = "AttackOrder", value = attOrder.ToString() });
            attributes.Add(new Attribute { trait_type = "Cost", value = cost.ToString() });
            attributes.Add(new Attribute { trait_type = "DetectsInvisibility", value = detectsInvisibility.ToString() });
            attributes.Add(new Attribute { trait_type = "ProjectileType", value = projType.ToString() });
            attributes.Add(new Attribute { trait_type = "ProjectileColor", value = projectileColor.ToString() });
            attributes.Add(new Attribute { trait_type = "TowerColor", value = towerColor.ToString() });
            attributes.Add(new Attribute { trait_type = "BackgroundColor", value = backgroundColor.ToString() });
            Dictionary<string, List<object>> properties = new Dictionary<string, List<object>>();
            properties["creators"] = new List<object> { new CreatorInfo() };
            properties["files"] = new List<object> { new FileInfo { uri = i + ".png" } };


            TowerMetaData tmd = new TowerMetaData
            {
                name = "KITCHEN-TOWER-" + i,
                symbol = "KT-"+i,
                description = "#" + i + ": " + towerColor.ToString() + " Tower with " + projectileColor.ToString() + " " + projType.ToString() + " projectiles on " + backgroundColor.ToString() + " background. "+ "Collection of " + towersToGenerate + " towers to be used in Kitchen Tower Defense Game on the website seth.kitchen. This is number " + i + ".",
                image = i + ".png",
                attributes = attributes,
                properties = properties,
                collection = new Collection()
            };

            string jsonData = JsonConvert.SerializeObject(tmd, Formatting.None);
            File.WriteAllText(@"D:\NFTs\" + i + ".json", jsonData);
            DrawImage(projType, attOrder, attackRange, attackSpeed, cost, detectsInvisibility, projectileColor, towerColor, backgroundColor, i);
        }
    }

    public static void Main(string[] args)
    {
        DoIt();
    }
}