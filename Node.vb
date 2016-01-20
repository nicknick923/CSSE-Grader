Public Class Node

    Public nextNode As Node
    Public info As Object

    Public Sub New(inInfo As Object, inNext As Node)
        info = inInfo
        nextNode = inNext
    End Sub

End Class
