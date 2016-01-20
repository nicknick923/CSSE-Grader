Public Class Stack

    Private top As Node

    Private Boolean isEmpty()
        top == null
    End Boolean
    Private Sub push(inString As String)
        top = New Node(inString, top)
    End Sub

    Private Function pop() As String
        If (isEmpty()) Then
            Return ""
    End Function
End Class
