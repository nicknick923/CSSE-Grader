Public Class Stack
    Private elements(10000) As Char
    Private top As Integer

    Public Sub New()
        For Each x As Char In elements
            x = vbNullChar
        Next
        top = 0
    End Sub
    Public Function isEmpty() As Boolean
        Return top = 0
    End Function

    Public Sub push(inChar As Char)
        elements(top) = inChar
        top += 1
    End Sub

    Public Function pop() As Char
        top -= 1
        Return elements(top)
    End Function

    Public Sub clear()
        top = 0
    End Sub
End Class
