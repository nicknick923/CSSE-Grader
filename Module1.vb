Module Module1
    Dim fileLocation As String = My.Application.CommandLineArgs.Item(0)
    Dim lineList As New List(Of String)
    Dim curLine As Integer = 0
    Dim indentLevel As Integer = 0
    Dim lineTemp As String
    Dim commentBlock As Boolean
    Dim extraIndent As String = ""
    Dim errorString As String = vbNullString
    Dim magicNumbers As List(Of Integer) = New List(Of Integer)
    Dim indentationErrors As List(Of Integer) = New List(Of Integer)

    Public Sub onMyLoad(sender As Object, e As EventArgs)

    End Sub

    Public Sub generateFile()
        errorString = "Magic Number Violations:" & vbCrLf
        For Each line As Integer In magicNumbers
            errorString &= " " & line
        Next
        errorString &= vbCrLf
        errorString &= "Indentation Violations:" & vbCrLf
        For Each line As Integer In indentationErrors
            errorString &= " " & line
        Next
        My.Computer.FileSystem.WriteAllText(My.Computer.FileSystem.CurrentDirectory & "/errors.txt", errorString, False)
    End Sub


    Public Sub prepFile()
        Dim fileString As String = My.Computer.FileSystem.ReadAllText(fileLocation)
        fileString = fileString.Replace(vbCrLf, vbLf)
        Dim lineArray() As String = fileString.Split(vbLf)
        For Each line As String In lineArray
            lineList.Add(line)
        Next
    End Sub

    Public Sub displayFile()
        Form1.TextBox1.Text = ""
        Dim textboxstring As String = vbNullString
        Dim lineNum As Integer = 1
        For Each line As String In lineList
            textboxstring &= lineNum & ": " & line & vbCrLf
            lineNum += 1
        Next
        Form1.TextBox1.Text = textboxstring
    End Sub

    Public Sub readLine()
        lineTemp = lineList(curLine)
        If lineTemp.Contains("/*") Or lineTemp.Contains("/*") Then
            commentBlock = True
        ElseIf curLine > 0 Then

            If lineList(curLine - 1).Contains("*/") Then
                commentBlock = False
            End If
        End If
        isIndentOk()
        If isMagicNumber() Then
            magicNumbers.Add(curLine + 1)
        End If
        curLine += 1
    End Sub

    Public Function atEnd() As Boolean
        Return curLine.Equals(lineList.Count)
    End Function

    Public Sub isLineAboveOpenCurlyBrace()
        If curLine > 0 Then
            If lineList(curLine - 1).Contains("{") Then
                indentLevel += 1
            End If
        End If
    End Sub

    Public Sub doesLineContainClosingCurlyBrace()
        If lineList(curLine).Contains("}") Then
            indentLevel -= 1
        End If
    End Sub

    Private Sub isIndentOk()
        doesLineContainClosingCurlyBrace()
        isLineAboveOpenCurlyBrace()
        isOneLineIndent()
        Dim firstNum As Integer = 0
        Dim secondNum As Integer = (3 * indentLevel)
        If commentBlock Then
            If Not (lineList(curLine).Length = 0) Then
                Dim expectedIndent As String = indentGenerator()
                Dim givenIndent As String = lineList(curLine).Substring(0, secondNum)
                If lineList(curLine).StartsWith(expectedIndent & " ") And Not lineList(curLine).StartsWith(expectedIndent & "/") Then
                    If lineList(curLine).StartsWith(expectedIndent & "  ") Then
                        indentationErrors.Add(curLine + 1)
                    End If
                End If
            End If
        Else
            If Not (lineList(curLine).Length = 0) Then
                Dim expectedIndent As String = indentGenerator()
                Dim givenIndent As String = lineList(curLine).Substring(0, secondNum)
                If lineList(curLine).StartsWith(expectedIndent & extraIndent) And Not lineList(curLine).StartsWith(expectedIndent & extraIndent & " ") Then
                Else
                    If Not lineTemp.Replace(" ", "").StartsWith("+") Then
                        If Not lineList(curLine - 1).EndsWith(",") Then
                            indentationErrors.Add(curLine + 1)
                        End If
                    End If
                End If
            End If
        End If
        extraIndent = ""
    End Sub

    Private Function indentGenerator() As String
        Dim retString As String = ""
        Dim i As Integer = 0
        While (i < indentLevel)
            retString &= "   "
            i += 1
        End While
        Return retString
    End Function

    Private Sub isOneLineIndent()
        Dim myCurLine As Integer = curLine
        If curLine > 0 And myCurLine > 0 Then
            While lineList(myCurLine - 1).Contains("if") Or lineList(myCurLine - 1).Contains("else") Or lineList(myCurLine - 1).Contains("for") Or lineList(myCurLine - 1).Contains("while")
                If Not lineTemp.Contains("{") Then
                    extraIndent &= "   "
                End If
                myCurLine -= 1
            End While
        End If
    End Sub

    Private Function isMagicNumber() As Boolean
        Dim containsNum As Boolean = False
        Dim prevChar As Char = vbNullChar
        For Each myChar As Char In lineTemp
            If Char.IsNumber(myChar) Then
                Select Case myChar
                    Case "0"
                        If Char.IsNumber(prevChar) Then
                            containsNum = True
                        End If
                    Case "1"
                        If Char.IsNumber(prevChar) Then
                            containsNum = True
                        End If
                    Case Else
                        containsNum = True
                End Select
            End If
            prevChar = myChar
        Next
        Return containsNum And Not lineTemp.Contains("final")
    End Function


End Module
