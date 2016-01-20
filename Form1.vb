Imports System.IO.File
Public Class Form1
    Private openingChar() As Char = {"{", "(", "["}
    Private closingChar() As Char = {"}", ")", "]"}

    Private ourStack As Stack = New Stack

    Private inCommentBlock As Boolean = False
    Private inCommentLine As Boolean = False

    Private indentationLevel As Integer = 0
    Private previousLine As String = ""
    Private oneLiner As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
        If OpenFileDialog1.CheckFileExists Then
            TextBox1.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenFileDialog1.FileName = TextBox1.Text
        OpenFileDialog1.FileName = "C:\Users\Jacob\Documents\1A.txt"
        gradeTheFile(OpenFileDialog1.FileName)
    End Sub
    Private Sub gradeTheFile(filePath As String)
        ourStack.clear()
        Dim fileString As String = My.Computer.FileSystem.ReadAllText(filePath)
        Dim fileStringLines As String() = fileString.Split(vbCrLf)
        Dim lineNumber As Integer = 0

        For Each line As String In fileStringLines
            line = line.Replace(vbLf, "")
            preCommentCode(line)
            preIndentationLevelCheck(line)
            lineNumber += 1
            If isLineTooLong(line) Then
                MsgBox("LINE TOO LONG!!!" & vbCrLf & line)
            End If
            If (Not inCommentBlock) And (Not inCommentLine) Then
                If indentationErrors(line) Then
                    MsgBox("INDENTATION ERROR!!!" & vbCrLf & lineNumber & ":" & line)
                End If
                For Each charactor As Char In line
                    If isOpeningChar(charactor) Then
                        ourStack.push(charactor)
                    End If
                    If isClosingChar(charactor) Then
                        If (Not ourStack.isEmpty()) Then
                            Dim tempChar = ourStack.pop()
                            If (Not tempChar = oppositeChar(charactor)) Then
                                MsgBox("UNMATCHED BRACES!!!" & vbCrLf & charactor & vbCrLf & lineNumber & ":" & line)
                            End If
                        Else
                            MsgBox("UNMATCHED BRACES!!!" & vbCrLf & charactor & vbCrLf & lineNumber & ":" & line)
                        End If
                    End If
                Next
            End If
            postIndentationLevelCheck(line)
            previousLine = line
        Next
    End Sub

    Private Function isLineTooLong(line As String) As Boolean
        Return line.Length > 74
    End Function

    Private Function isOpeningChar(inChar As Char) As Boolean
        Select Case inChar
            Case "("c
                Return True
            Case "{"c
                Return True
            Case "["c
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function isClosingChar(inChar As Char) As Boolean
        Select Case inChar
            Case ")"c
                Return True
            Case "}"c
                Return True
            Case "]"c
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Sub preIndentationLevelCheck(line As String)

        If (Not inCommentBlock) And (Not inCommentLine) Then
            If line.Replace(" ", "") = "}" Then
                indentationLevel -= 1
            End If
            If previousLine.Contains("for") Or previousLine.Contains("if") Or previousLine.Contains("else") Or previousLine.Contains("while") Then
                If Not line.Contains("{") Then
                    indentationLevel += 1
                    oneLiner = True
                End If
            End If
        End If
    End Sub

    Private Sub postIndentationLevelCheck(line As String)
        If (Not inCommentBlock) And (Not inCommentLine) Then
            If previousLine.Contains("for") Or previousLine.Contains("if") Or previousLine.Contains("else") Or previousLine.Contains("while") Then
                If line.Contains("{") Then
                    indentationLevel += 1
                End If
            End If
            If oneLiner Then
                indentationLevel -= 1
                oneLiner = False
            End If
        End If
    End Sub

    Private Function oppositeChar(inChar As Char) As Char
        Select Case inChar
            Case "{"c
                Return "}"c
            Case "["c
                Return "]"c
            Case "("c
                Return ")"c
            Case "}"c
                Return "{"c
            Case "]"c
                Return "["c
            Case ")"c
                Return "("c
            Case Else
                Return vbNullChar
        End Select
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OpenFileDialog1.FileName = "C:\Users\Jacob\Documents\1A.txt"
    End Sub

    Private Sub preCommentCode(line As String)
        inCommentLine = line.Replace(" ", "").StartsWith("//")
        If line.Replace(" ", "").StartsWith("/**") Then
            inCommentBlock = True
        End If
    End Sub
    Private Sub postCommentCode(line As String)
        If line.Replace(" ", "").EndsWith("*/") Then
            inCommentBlock = False
        End If
    End Sub

    Private Function indentationErrors(line As String) As Boolean

        If line.StartsWith(indentationReturn()) Then
            If indentationLevel > 0 And line.Replace(indentationReturn(), "").Length > 0 Then
                If (Not line.Replace(indentationReturn(), "").First() = " ") Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Private Function indentationReturn() As String
        Dim retString As String = ""
        Dim x As Integer = 0
        While x < indentationLevel
            retString &= "   "
            x += 1
        End While
        Return retString
    End Function

End Class
