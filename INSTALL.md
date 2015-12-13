#Compiling
To compile this program under Windows you will need a version of Visual Studio that can support C# 6.0 (2015 is just fine) or the latest version of Xamarin Studio. On Linux or OS X you will have to get the latest version of MonoDevelop or Xamarin Studio, or one that supports all the features of C# 6.0

#### Get the code
If you have git, you can simply clone it by using this command:
```
git clone https://github.com/wallnutkraken/ClTwitterEbooks.git
```
Or you could download a zip file of the current master branch [here](https://github.com/wallnutkraken/ClTwitterEbooks/archive/master.zip).

#### Dependencies
Normally, Visual Studio or Xamarin will restore the packages that the project depends on when you first start it. However, if that doesn't happen, you will have to install these packages yourself. These are the commands for NuGet to get the packages required:
```
Install-Package ClutteredMarkovGenerator
Install-Package TweetMoaSharp 
```

#### Actually compiling it
Open up the solution in whichever C# IDE you use and compile it.

# First run

On the first run you will encounter a scary message that refers you to read the documentation (it meant this file in particular). This section will provide relevant information and detailed explaination for these steps. Buckle up.

###Would you like to use a custom API key? [Y/N]
By default, this application will use the default API key. There's some more information on that [here](https://dev.twitter.com/oauth/overview/faq).

 - go to [apps.twitter.com](https://apps.twitter.com/)
 - sign in to your twitter account
 - press Create New App
 - fill in the information Twitter asks you for and press Create your Twitter application
 - go back to the main screen of [apps.twitter.com](https://apps.twitter.com/)
 - click on your newly-created app
 - press the "Keys and Access tokens" tab
 - copy your API keys/tokens to the application

### The default path to look for the twitter archive .js files is the 'twitter' directory where this executable is located.
First, if you don't have your twitter archive downloaded yet, [DO SO](https://support.twitter.com/articles/20170160?lang=en#).
Once you have downloaded it and unzipped the file, there will be a few directories. Now you have two choices. You can either copy the folder 'data\js\tweets' from your Twitter archive to the folder where you have the executable of this file. Alternatively, you can press Y here and and give the application either the [absolute or relative](https://en.wikipedia.org/wiki/Path_%28computing%29#Absolute_and_relative_paths) path to this directory.


### Please enter the username of the twitter account which you want to use to feed this bot information.

Most of the time, that account from which you are getting the Tweets to make the Markov chain is not the same as the one posting. Just type in the username of the source user. If you mess up, and type in a username that does not exist, we'll give you another chance.

###Do you want to any optional settings? [Y/N]
These settings are completely optional and if you don't set them, defaults will be used. This guide will not cover the details of these settings.