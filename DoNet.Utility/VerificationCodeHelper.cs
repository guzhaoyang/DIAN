// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.VerificationCodeHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Web;

namespace DoNet.Utility
{
  public class VerificationCodeHelper
  {
    private readonly int letterWidth = 16;
    private readonly int letterHeight = 20;
    private static readonly byte[] Randb = new byte[4];
    private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
    private readonly Font[] _fonts = new Font[4]
    {
      new Font(new FontFamily("Times New Roman"), (float) (10 + VerificationCodeHelper.Next(1)), FontStyle.Regular),
      new Font(new FontFamily("Georgia"), (float) (10 + VerificationCodeHelper.Next(1)), FontStyle.Regular),
      new Font(new FontFamily("Arial"), (float) (10 + VerificationCodeHelper.Next(1)), FontStyle.Regular),
      new Font(new FontFamily("Comic Sans MS"), (float) (10 + VerificationCodeHelper.Next(1)), FontStyle.Regular)
    };

    public VerificationCodeHelper()
    {
      HttpContext.Current.Response.Expires = 0;
      HttpContext.Current.Response.Buffer = true;
      HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
      HttpContext.Current.Response.AddHeader("pragma", "no-cache");
      HttpContext.Current.Response.CacheControl = "no-cache";
      this.Text = VerificationCode.Number(4);
      this.CreateImage();
    }

    public string Text { get; }

    public Bitmap Image { get; private set; }

    private static int Next(int max)
    {
      VerificationCodeHelper.Rand.GetBytes(VerificationCodeHelper.Randb);
      int num = BitConverter.ToInt32(VerificationCodeHelper.Randb, 0) % (max + 1);
      if (num < 0)
        num = -num;
      return num;
    }

    private static int Next(int min, int max) => VerificationCodeHelper.Next(max - min) + min;

    public void CreateImage()
    {
      int width = this.Text.Length * this.letterWidth;
      Bitmap srcBmp = new Bitmap(width, this.letterHeight);
      Graphics graphics = Graphics.FromImage((System.Drawing.Image) srcBmp);
      graphics.Clear(Color.White);
      for (int index = 0; index < 2; ++index)
      {
        int x1 = VerificationCodeHelper.Next(srcBmp.Width - 1);
        int x2 = VerificationCodeHelper.Next(srcBmp.Width - 1);
        int y1 = VerificationCodeHelper.Next(srcBmp.Height - 1);
        int y2 = VerificationCodeHelper.Next(srcBmp.Height - 1);
        graphics.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
      }
      int x3 = -12;
      for (int startIndex = 0; startIndex < this.Text.Length; ++startIndex)
      {
        x3 += VerificationCodeHelper.Next(12, 16);
        int y = VerificationCodeHelper.Next(-2, 2);
        string str = this.Text.Substring(startIndex, 1);
        string s = VerificationCodeHelper.Next(1) == 1 ? str.ToLower() : str.ToUpper();
        Brush brush = (Brush) new SolidBrush(this.GetRandomColor());
        Point point = new Point(x3, y);
        graphics.DrawString(s, this._fonts[VerificationCodeHelper.Next(this._fonts.Length - 1)], brush, (PointF) point);
      }
      for (int index = 0; index < 10; ++index)
      {
        int x4 = VerificationCodeHelper.Next(srcBmp.Width - 1);
        int y = VerificationCodeHelper.Next(srcBmp.Height - 1);
        srcBmp.SetPixel(x4, y, Color.FromArgb(VerificationCodeHelper.Next(0, (int) byte.MaxValue), VerificationCodeHelper.Next(0, (int) byte.MaxValue), VerificationCodeHelper.Next(0, (int) byte.MaxValue)));
      }
      Bitmap bitmap = this.TwistImage(srcBmp, true, (double) VerificationCodeHelper.Next(1, 3), (double) VerificationCodeHelper.Next(4, 6));
      graphics.DrawRectangle(new Pen(Color.LightGray, 1f), 0, 0, width - 1, this.letterHeight - 1);
      this.Image = bitmap;
    }

    public Color GetRandomColor()
    {
      Random random1 = new Random((int) DateTime.Now.Ticks);
      Thread.Sleep(random1.Next(50));
      Random random2 = new Random((int) DateTime.Now.Ticks);
      int red = random1.Next(180);
      int green = random2.Next(180);
      int num = red + green > 300 ? 0 : 400 - red - green;
      int blue = num > (int) byte.MaxValue ? (int) byte.MaxValue : num;
      return Color.FromArgb(red, green, blue);
    }

    public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
    {
      double num1 = 2.0 * Math.PI;
      Bitmap bitmap = new Bitmap(srcBmp.Width, srcBmp.Height);
      Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap);
      graphics.FillRectangle((Brush) new SolidBrush(Color.White), 0, 0, bitmap.Width, bitmap.Height);
      graphics.Dispose();
      double num2 = bXDir ? (double) bitmap.Height : (double) bitmap.Width;
      for (int x1 = 0; x1 < bitmap.Width; ++x1)
      {
        for (int y1 = 0; y1 < bitmap.Height; ++y1)
        {
          double num3 = Math.Sin((bXDir ? num1 * (double) y1 / num2 : num1 * (double) x1 / num2) + dPhase);
          int x2 = bXDir ? x1 + (int) (num3 * dMultValue) : x1;
          int y2 = bXDir ? y1 : y1 + (int) (num3 * dMultValue);
          Color pixel = srcBmp.GetPixel(x1, y1);
          if (x2 >= 0 && x2 < bitmap.Width && y2 >= 0 && y2 < bitmap.Height)
            bitmap.SetPixel(x2, y2, pixel);
        }
      }
      srcBmp.Dispose();
      return bitmap;
    }
  }
}
