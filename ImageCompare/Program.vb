Imports System.Drawing
Imports Microsoft.VisualBasic.Imaging

Module Program

    Sub Main()
        Dim a = Image.FromFile("F:\asuka\computer-vision\data\a.png")
        Dim b = Image.FromFile("F:\asuka\computer-vision\data\b.png")
        Dim rsult = Compares.Compares(a, b)

        Call DrawQuery(a, rsult).SaveAs("F:\asuka\computer-vision\data\a_test.png", ImageFormats.Png)
        Call DrawHit(b, rsult).SaveAs("F:\asuka\computer-vision\data\b_test.png", ImageFormats.Png)
    End Sub
End Module
