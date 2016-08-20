Imports System.Net
Imports System.Environment
Imports MaterialDesignThemes
Imports System.ComponentModel
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports MetroFramework

Public Class Form1
    Dim PortAIOCommonGitVersion As String
    Dim PortAIOGitVersion As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MaterialCheckBox1.Visible = True
        MaterialCheckBox2.Visible = True
        MaterialCheckBox1.Enabled = False
        MaterialCheckBox2.Enabled = False
        MaterialCheckBox3.Visible = True
        Dim client As WebClient = New WebClient()
        PortAIOCommonGitVersion = client.DownloadString("https://raw.githubusercontent.com/berbb/PortAIO-Updater/master/PortAIO.Common.version")
        PortAIOGitVersion = client.DownloadString("https://raw.githubusercontent.com/berbb/PortAIO-Updater/master/PortAIO.version")
        CheckUpdatePortAIO_Common()
    End Sub

    Private Sub CheckUpdatePortAIO_Common()
        Dim PortAIOCommon As String = GetFolderPath(SpecialFolder.ApplicationData) + "\EloBuddy\Addons\Libraries\PortAIO.Common.dll"
        If System.IO.File.Exists(PortAIOCommon) Then
            Dim VersionPortCommon As FileVersionInfo = FileVersionInfo.GetVersionInfo(PortAIOCommon)
            If VersionPortCommon.FileVersion <> PortAIOCommonGitVersion Then
                Dim result As Integer = MetroMessageBox.Show(Me, "A new version is available, please download the new update!", "Version Check", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand)
                If result = DialogResult.Cancel Then
                    CheckUpdatePortAIO()
                    MetroLabel5.Text = "Lib Stataus : Outdated"
                    MaterialCheckBox3.Visible = True
                ElseIf result = DialogResult.OK Then
                    DeleteFile()
                    DownloadFile1()
                End If
            Else
                MetroLabel5.Text = "Lib Stataus : UPDATED"
                'MetroLabel3.Text = "UPDATED"
                MaterialProgressBar1.Value = 100
                CheckUpdatePortAIO()
            End If
        Else
            MetroLabel5.Text = "Lib Stataus : Not found"
            Dim result As Integer = MetroMessageBox.Show(Me, "PortAIO.Common.Dll doesn't exist, please press OK to download it.", "File Check", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            If result = DialogResult.Cancel Then
                'MessageBox.Show("Need Download To Play")
                CheckUpdatePortAIO()
                MaterialCheckBox3.Visible = True
            ElseIf result = DialogResult.OK Then
                DownloadFile1()
            End If
        End If
    End Sub
    Private Sub CheckUpdatePortAIO()
        Dim PortAIO As String = GetFolderPath(SpecialFolder.ApplicationData) + "\EloBuddy\Addons\Libraries\PortAIO.dll"
        If System.IO.File.Exists(PortAIO) Then
            Dim VersionPort As FileVersionInfo = FileVersionInfo.GetVersionInfo(PortAIO)
            If VersionPort.FileVersion <> PortAIOGitVersion Then
                Dim result As Integer = MetroMessageBox.Show(Me, "A new version is available, please download the new update!", "Version Check", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand)
                If result = DialogResult.Cancel Then
                    MetroLabel6.Text = "PortAIO Status : Outdated"
                    MaterialCheckBox3.Visible = True
                ElseIf result = DialogResult.OK Then
                    DeleteFile()
                    DownloadFile2()
                End If
            Else
                MetroLabel6.Text = "PortAIO Status : UPDATED"
                'MetroLabel4.Text = "downloading ..."
                MaterialProgressBar2.Value = 100
                MaterialCheckBox3.Visible = True
                MetroProgressSpinner1.Visible = False
            End If
        Else
            MetroLabel6.Text = "PortAIO Status : Not found"
            If MaterialCheckBox1.Checked = False And MaterialCheckBox2.Checked = False Then
                Dim result As Integer = MetroMessageBox.Show(Me, "PortAIO.Dll doesn't exist, please press OK to download it.", "File Check", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                If result = DialogResult.Cancel Then
                    MaterialCheckBox3.Visible = True
                ElseIf result = DialogResult.OK And MaterialCheckBox3.Checked = False Then
                    DownloadFile2() ' auto download
                ElseIf MaterialCheckBox3.Checked Then
                    DownloadFile2() ' manual download 
                End If
            ElseIf MaterialCheckBox1.Checked And MaterialCheckBox2.Checked Then
                If MaterialCheckBox3.Checked Then
                    DownloadFile2()
                End If
            End If
        End If
    End Sub

    Private Sub autoupdateGitHub()
        Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
        Dim appDataElo As String = appData + "\EloBuddy\Addons\Libraries\PortAIO.Common.dll"
        '    md5code2 = md5_hash(appDataElo)
    End Sub

    Private Sub ShowDownloadProgress1(sender As Object, e As DownloadProgressChangedEventArgs)
        MaterialProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub DownloadFile1()
        MetroLabel3.Text = "downloading ..."
        Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
        Dim appDataElo As String = appData + "\EloBuddy\Addons\Libraries\PortAIO.Common.dll"
        Dim client As New WebClient()
        AddHandler client.DownloadProgressChanged, AddressOf ShowDownloadProgress1
        AddHandler client.DownloadFileCompleted, AddressOf OnDownloadComplete1
        client.DownloadFileAsync(New Uri("https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.Common.dll"), appDataElo)
    End Sub

    Private Sub DownloadFile2()
        MetroLabel4.Text = "downloading ..."
        Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
        Dim appDataElo As String = appData + "\EloBuddy\Addons\Libraries\PortAIO.dll"
        Dim client As New WebClient()
        AddHandler client.DownloadProgressChanged, AddressOf ShowDownloadProgress2
        AddHandler client.DownloadFileCompleted, AddressOf OnDownloadComplete2
        client.DownloadFileAsync(New Uri("https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.dll"), appDataElo)
    End Sub

    Private Sub OnDownloadComplete1(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        Dim PortAIOCommon As String = GetFolderPath(SpecialFolder.ApplicationData) + "\EloBuddy\Addons\Libraries\PortAIO.Common.dll"
        Dim VersionPortCommon As FileVersionInfo = FileVersionInfo.GetVersionInfo(PortAIOCommon)
        If Not e.Cancelled AndAlso e.Error Is Nothing Then
            If VersionPortCommon.FileVersion = PortAIOCommonGitVersion Then
                MetroLabel5.Text = "Lib Stataus : UPDATED"
                MetroLabel3.Text = "download done.."
                CheckUpdatePortAIO()
            Else
                MessageBox.Show("Download failed")
            End If
        End If
    End Sub

    Private Sub OnDownloadComplete2(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        Dim PortAIO As String = GetFolderPath(SpecialFolder.ApplicationData) + "\EloBuddy\Addons\Libraries\PortAIO.dll"
        Dim VersionPort As FileVersionInfo = FileVersionInfo.GetVersionInfo(PortAIO)
        If Not e.Cancelled AndAlso e.Error Is Nothing Then
            If VersionPort.FileVersion = PortAIOGitVersion Then
                MetroLabel6.Text = "PortAIO Stataus : UPDATED"
                MetroLabel4.Text = "download done.."
                MaterialCheckBox3.Enabled = True
            Else
                MessageBox.Show("Download failed")
            End If
        End If
    End Sub

    Private Sub ShowDownloadProgress2(sender As Object, e As DownloadProgressChangedEventArgs)
        MaterialProgressBar2.Value = e.ProgressPercentage
    End Sub

    Private Sub DeleteFile()
        Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
        Dim PortAIO_Common As String = appData + "\EloBuddy\Addons\Libraries\PortAIO.Common.dll"
        If System.IO.File.Exists(PortAIO_Common) Then
            System.IO.File.Delete(PortAIO_Common)
        Else
            MsgBox("Doesn't exist", 64, "Open")
        End If

        Dim PortAIO As String = appData + "\EloBuddy\Addons\Libraries\PortAIO.dll"
        If System.IO.File.Exists(PortAIO) Then
            System.IO.File.Delete(PortAIO)
        Else
            MsgBox("Doesn't exist", 64, "Open")
        End If
    End Sub

    Private Sub MaterialFlatButton1_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton1.Click
        If MaterialCheckBox1.Checked = True And MaterialCheckBox2.Checked = False Then
            DownloadFile1()
        End If
        If MaterialCheckBox2.Checked And MaterialCheckBox1.Checked = False Then
            DownloadFile2()
        End If
        If MaterialCheckBox1.Checked And MaterialCheckBox2.Checked Then
            MetroMessageBox.Show(Me, "Please select only one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question)
        End If
        If MaterialCheckBox1.Checked = False And MaterialCheckBox2.Checked = False Then
            MetroMessageBox.Show(Me, "Please choose the DLLs you want to download.", "None Selected", MessageBoxButtons.OK, MessageBoxIcon.Question)
        End If
    End Sub

    Private Sub MaterialFlatButton2_Click(sender As Object, e As EventArgs) Handles MaterialFlatButton4.Click
        Dim webAddress As String = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=DZSQBFWWV9WEN"
        Process.Start(webAddress)
    End Sub

    Private Sub MaterialCheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialCheckBox3.CheckedChanged
        If MaterialCheckBox3.Checked Then
            MaterialCheckBox1.Visible = True
            MaterialCheckBox2.Visible = True
            MaterialCheckBox1.Enabled = True
            MaterialCheckBox2.Enabled = True
            MaterialFlatButton1.Enabled = True
        Else
            MaterialFlatButton1.Enabled = False
            MaterialCheckBox1.Enabled = False
            MaterialCheckBox2.Enabled = False
        End If

    End Sub

    Private Sub MetroLink1_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=DZSQBFWWV9WEN"
        Process.Start(webAddress)
    End Sub

    Private Sub MaterialCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialCheckBox1.CheckedChanged
        If MaterialCheckBox1.Checked Then
            MaterialCheckBox2.Checked = False
        End If
    End Sub

    Private Sub MaterialCheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles MaterialCheckBox2.CheckedChanged
        If MaterialCheckBox2.Checked Then
            MaterialCheckBox1.Checked = False
        End If
    End Sub
End Class
