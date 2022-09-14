cmake_minimum_required(VERSION 3.20)

project(main)

add_library(main main.cpp)

# set(CMAKE_OSX_ARCHITECTURES "x86_64")
# set(CMAKE_SYSROOT_COMPILE "/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneSimulator.platform/Developer/SDKs/iPhoneSimulator15.2.sdk")
# set(CMAKE_CXX_FLAGS "")

set_target_properties(main PROPERTIES COMPILE_FLAGS "--sysroot /Applications/Xcode.app/Contents/Developer/Platforms/iPhoneSimulator.platform/Developer/SDKs/iPhoneSimulator15.2.sdk")


set_target_properties(main PROPERTIES
    BUNDLE True
    MACOSX_BUNDLE_GUI_IDENTIFIER com.nucleus.main
    MACOSX_BUNDLE_BUNDLE_NAME main
    MACOSX_BUNDLE_BUNDLE_VERSION "0.1"
    MACOSX_BUNDLE_SHORT_VERSION_STRING "0.1"
	XCODE_ATTRIBUTE_CODE_SIGN_IDENTITY "iPhone Developer: Nucleus"
)
    # MACOSX_BUNDLE_INFO_PLIST ${CMAKE_SOURCE_DIR}/cmake/customtemplate.plist.in
