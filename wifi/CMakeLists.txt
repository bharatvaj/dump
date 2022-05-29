cmake_minimum_required(VERSION 3.17)
cmake_policy(SET CMP0091 NEW)
project(swifi VERSION 0.1.0 LANGUAGES C DESCRIPTION "Wifi for windows")
set(CMAKE_CXX_STANDARD 17)

add_library(swifi wifi.c)
target_include_directories(swifi PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})

add_executable(swifi_test wifi_test.c)
target_link_libraries(swifi_test PRIVATE swifi)

#enable_test()
#add_test(swifi_test)
