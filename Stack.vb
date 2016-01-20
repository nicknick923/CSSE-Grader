Public Class Stack
    Private elements(10000) As String
    Private top As Integer

    Public Sub New()
        For Each x As String In elements
            x = vbNullString
        Next
        top = 0
    End Sub
    Private Function isEmpty() As Boolean
        Return top = 0
    End Function

    Private Sub push(inString As String)
        elements(top) = inString
        top += 1
    End Sub

    Private Function pop() As String
        top -= 1
        Return elements(top)
    End Function

    Private Sub clear()
        top = 0
    End Sub
End Class
