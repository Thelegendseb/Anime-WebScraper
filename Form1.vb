Imports PuppeteerSharp
Imports Guna
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Async Function GetAnimeStats(LinkForTask As String) As Task(Of Anime)

        Dim SiteNum As Byte = GetSiteNum(LinkForTask)


        Dim AnimeToReturn As New Anime

        Dim browser = Await Puppeteer.LaunchAsync(New LaunchOptions With {.Headless = False, .ExecutablePath = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"})
        'value for progress bar
        CircleProg.Value += 10

        Me.BringToFront()

        Dim page = Await browser.NewPageAsync()
        CircleProg.Value += 10
        Await page.GoToAsync(LinkForTask)
        'value for progress bar
        CircleProg.Value += 10

        Dim genres
        Dim Title
        Dim Bio
        Dim imageurl

        '====================================
        Select Case SiteNum
            Case 1   'AnimixPlay.to
                '====================================

                Title = Await page.WaitForSelectorAsync("#animepagetitle")
                'value for progress bar
                CircleProg.Value += 10
                Bio = Await page.WaitForSelectorAsync("#panelplace")
                'value for progress bar
                CircleProg.Value += 10
                genres = Await page.QuerySelectorAllAsync("a[href*='genre']")
                'value for progress bar
                CircleProg.Value += 10
                imageurl = Await page.WaitForSelectorAsync("#maincoverimage")
                'value for progress bar
                CircleProg.Value += 10
                '====================================
                For Each element In genres
                    AnimeToReturn.Genres.Add(Await page.EvaluateFunctionAsync(Of String)("e => e.textContent", element))  'element
                Next
                'value for progress bar
                CircleProg.Value += 10
                AnimeToReturn.Name = Await page.EvaluateFunctionAsync(Of String)("e => e.textContent", Title)
                'value for progress bar
                CircleProg.Value += 10
                AnimeToReturn.Bio = Await page.EvaluateFunctionAsync(Of String)("e => e.textContent", Bio)
                'value for progress bar
                CircleProg.Value += 10
                AnimeToReturn.ImageLink = Await page.EvaluateFunctionAsync(Of String)("e => e.textContent", imageurl)
                'value for progress bar
                CircleProg.Value += 10
                '====================================
            Case 2 'AniList
                '====================================

                Title = Await page.WaitForSelectorAsync("h1")
                'value for progress bar
                CircleProg.Value += 10
                Bio = Await page.WaitForSelectorAsync(".description")       '.content
                'value for progress bar 
                CircleProg.Value += 10
                genres = Await page.QuerySelectorAllAsync("a[href*='/search/anime/']")
                'value for progress bar
                CircleProg.Value += 10
                '====================================
                For Each element In genres
                    AnimeToReturn.Genres.Add(Await page.EvaluateFunctionAsync(Of String)("e => e.textContent", element))
                Next
                'value for progress bar
                CircleProg.Value += 10
                AnimeToReturn.Name = (Await page.EvaluateFunctionAsync(Of String)("e => e.textContent", Title)).Trim

                'value for progress bar
                CircleProg.Value += 10
                AnimeToReturn.Bio = Await page.EvaluateFunctionAsync(Of String)("e => e.textContent", Bio)
                'value for progress bar
                CircleProg.Value += 10
        End Select

        Await browser.CloseAsync()


        Return AnimeToReturn

    End Function

    Function GetSiteNum(Link As String) As Integer
        If InStr(Link, "https://animixplay.to/") <> 0 Then
            Return 1
        ElseIf InStr(Link, "https://anilist.co/") <> 0 Then
            Return 2
        End If
    End Function

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click

        Dim TheAnime As Anime = GetAnimeStats("https://animixplay.to/anime/47778/Kimetsu_no_Yaiba__Yuukaku-hen").GetAwaiter().GetResult()

    End Sub
End Class
