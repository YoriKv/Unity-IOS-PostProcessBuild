Unity-IOS-PostProcessBuild
==========================

Copy contents of "Editor" to your "Editor" folder under Unity Project Assets.

Requires ios-deploy, compiled and executable copied to /usr/bin/ios-deploy. Available here:
https://github.com/phonegap/ios-deploy

Requires 'xcodeproj' ruby gem. I found very little/poor documentation on this gem, required quite a bit of poking around the source and checking instance_variables in irb.

$ sudo gem install xcodeproj

Ruby sometimes doesn't pick up that the gem was installed. Might require a restart to guarantee that it refreshed.

When debugging, use Utils -> Test Build PostProcess to re-run the post processing step without doing a full build.

To Build and Run, use Utils -> Build and Run IOS.

For Append instead of Replace when building the XCode project from Unity3D. Replace "BuildOptions.None" with "BuildOptions.AcceptExternalModificationsToPlayer".


==========================

Special Thanks:

AsherVo - https://twitter.com/AsherVo - For making his own build/postprocess script public giving me lots of ideas, base code, and the inspiration to go hunting for other ways to make things work.

Akisute - http://akisute.com - For the original ruby post process script that gave me some ideas and through which I found out about the xcodeproj ruby gem. (https://gist.github.com/akisute/3780235)
