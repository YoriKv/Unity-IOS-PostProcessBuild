Unity-IOS-PostProcessBuild
==========================

Copy contents of "Editor" to your "Editor" folder under Unity Project Assets.

Requires 'xcodeproj' ruby gem. I found very little/poor documentation on this gem, required quite a bit of poking around the source and checking instance_variables in irb.

$ sudo gem install xcodeproj

Ruby sometimes doesn't pick up that the gem was installed. Might require a restart to guarantee that it refreshed.

When debugging, use Utils -> Test Build PostProcess to re-run the post processing step without doing a full build.
