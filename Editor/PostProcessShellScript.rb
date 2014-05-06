#!/usr/bin/env ruby
require 'xcodeproj'
require 'pathname'

# Make sure build path was provided
unless ARGV[0]
  abort("ERROR - Script must be run by unity or provide a build directory path")
end

#
# Get build directory path
#
build_path = ARGV[0]
puts "PostprocessBuildPlayer running on build directory: " + build_path

#
# Get project file
#
projpath = build_path + "/Unity-iPhone.xcodeproj"
proj = Xcodeproj::Project.open(projpath)

#
# Perform changes on XCode project file - Modify these lines to fit your needs
#

# Add system framework - StoreKit - This was a requirement for my own project, remove/add as necessary
proj.targets[0].add_system_framework("StoreKit")
# Set debug information to DWARF - makes for significantly faster build times, but should not be set for release builds
proj.targets[0].build_configuration_list.set_setting("DEBUG_INFORMATION_FORMAT", "DWARF")

#
# Save project file
#
proj.save(projpath)
