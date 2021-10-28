cmake_minimum_required(VERSION 3.3)

project(xbee LANGUAGES C)

file(GLOB_RECURSE xbee_HDRS "include/*.h")
file(GLOB_RECURSE xbee_SRCS "src/util/*.c" "src/posix/*.c" "src/wpan/*.c" "src/xbee/*.c" "src/zigbee/*.c")

add_library(xbee ${xbee_HDRS} ${xbee_SRCS})

target_compile_definitions(xbee PUBLIC POSIX)
target_include_directories(xbee PUBLIC
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>)
