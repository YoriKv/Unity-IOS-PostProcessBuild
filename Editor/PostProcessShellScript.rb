#!/usr/bin/env ruby
require 'rubygems'
require 'xcodeproj'

# Make sure build path was provided
unless ARGV[0]
    abort("ERROR - Script must be run by unity or provide a build directory path")
end

#
# Get build directory path
#
build_path = ARGV[0]

#
# Get project file
#
projpath = build_path + "/Unity-iPhone.xcodeproj"
proj = Xcodeproj::Project.open(projpath)

#
# Perform changes on XCode project file
#

# Add system framework - StoreKit
proj.targets[0].add_system_framework("StoreKit")
# Set debug information to DWARF - makes for significantly faster build times, but should not be set for release builds
proj.targets[0].build_configuration_list.set_setting("DEBUG_INFORMATION_FORMAT", "DWARF")

#
# Save project file
#
proj.save(projpath)

# Build
Dir.chdir build_path
system("xcodebuild")

# Deploy
system("ios-deploy -I -d -b build/" + ARGV[1] + ".app")
