# Learn languages with notifications
 
 ## What is this repo?
 This repository includes the entire source code for the application [__Learn German with notifications__](https://play.google.com/store/apps/details?id=com.grimar2008.notify_de). 

 It was written with __Xamarin__.

 _Download the application and rate it if you find this repo useful._

 ## About the App

 Lots of __language learning applications__ show you a notification telling you you should spend a few minutes in their App to learn new words. This App simply shows you a new word and a translation for it whenever you set the alarm for it via a notification.

 You are only expected to set up the required times once and never to open it again, unless you want to change the notification times or run through a list of already notified words.

 You can also choose which level (A1, A2, B1, B2, Advanced) you are.

 ## Technologies

 The App is written with __Xamarin__. There are some NuGets handling SQLite and Zip files.

 The app has a __SplashScreen__ which handles the database check to see whether the right language DB is being used. For notifications the combination of __Foreground Services__ and __AlarmManager__ are being used.

 ## Bugs

 There are too many android versions to keep track of, surely there will be some.
 
Pull requests are welcome.
