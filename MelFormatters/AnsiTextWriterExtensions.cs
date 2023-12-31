using System.Text;

namespace MelFormatters;

public static class AnsiTextWriterExtensions
{
    /// <summary>
    /// Calling with no args calls reset, equivalent to u001b[0m
    /// </summary>
    /// <remarks>
    /// <para>
    /// Color is two args.. eg bold yellow foreground is 93; basic yellow background is 43.
    /// Least significant digit is a colour, the rest determines fg, bg, basic, and bright.
    /// </para>
    /// <list type="table">
    ///   <listheader><term>msb</term><description>sets</description></listheader>
    ///   <item><term>3</term><description>basic foreground</description></item>
    ///   <item><term>9</term><description>bright foreground</description></item>
    ///   <item><term>4</term><description>basic background</description></item>
    ///   <item><term>10</term><description>bright background</description></item>
    /// </list>
    /// <list type="table">
    ///   <listheader><term>lsb</term><description>sets color</description></listheader>
    ///   <item><term>0</term><description>black</description></item>
    ///   <item><term>1</term><description>red</description></item>
    ///   <item><term>2</term><description>green</description></item>
    ///   <item><term>3</term><description>yellow</description></item>
    ///   <item><term>4</term><description>blue</description></item>
    ///   <item><term>5</term><description>magenta</description></item>
    ///   <item><term>6</term><description>cyan</description></item>
    ///   <item><term>7</term><description>white</description></item>
    /// </list>
    /// </remarks>
    /// <param name="fg">Use 30-37 or 90-97</param>
    /// <param name="bg">Use 40-47 or 100-107</param>
    /// <param name="text">0=reset, 1=bold, 3=italic, 4=underline
    /// </param>
    public static void Style(this TextWriter wr, byte? fg = null, byte? bg = null, byte text = 0)
    {
        var sb = new StringBuilder();
        sb.Append("\u001b[").Append(text);
        if (bg != null) sb.Append(';').Append(bg);
        if (fg != null) sb.Append(';').Append(fg);
        sb.Append('m');
        wr.Write(sb.ToString());
    }
}