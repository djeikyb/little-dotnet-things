using System.Text;

namespace Merviche.Mel.Formatter;

public static class AnsiTextWriterExtensions
{
    // https://vt100.net/docs/vt510-rm/SGR.html
    // https://notes.burke.libbey.me/ansi-escape-codes/
    // https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797
    // https://en.wikipedia.org/wiki/ANSI_escape_code
    // https://www.lihaoyi.com/post/BuildyourownCommandLinewithANSIescapecodes.html#8-colors
    // https://en.wikipedia.org/wiki/ANSI_escape_code#8-bit
    // https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#cursor-positioning
    // https://duffney.io/usingansiescapesequencespowershell/#basic-foreground--background-colors

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

    /// <inheritdoc cref="Style(System.IO.TextWriter,System.Nullable{byte},System.Nullable{byte},byte)"/>
    public static void Style(this TextWriter wr, params byte[] textFormatCodes)
    {
        if (textFormatCodes.Length == 0)
        {
            wr.Write("\x1b[0m");
            return;
        }

        var sb = new StringBuilder();
        sb.Append("\x1b[");
        sb.Append(textFormatCodes[0]);
        for (var index = 1; index < textFormatCodes.Length; index++)
        {
            var code = textFormatCodes[index];
            sb.Append(';').Append(code);
        }

        sb.Append('m');
        wr.Write(sb.ToString());
    }
}