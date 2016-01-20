Public Class Stack

    Private top As Node

    Private Function isEmpty() As Boolean
        Return top.Equals(vbNull)
    End Function

    Private Sub push(inString As String)
        top = New Node(inString, top)
    End Sub

    Private Function pop() As String
        If (isEmpty()) Then
            Return vbNullString
        Else

        End If
    End Function
End Class
