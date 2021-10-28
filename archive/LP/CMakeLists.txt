cmake_minimum_required(VERSION 3.0)
project(lp VERSION 0.5 DESCRIPTION "Lazy - A Session aware protocol")

file(GLOB_RECURSE lp_HDRS "include/*.h")
file(GLOB_RECURSE lp_SRCS "src/*.cpp")

add_library(lp STATIC ${lp_HDRS} ${lp_SRCS})
set_target_properties(lp PROPERTIES VERSION ${PROJECT_VERSION})
set_target_properties(lp PROPERTIES SOVERSION 1)
set_target_properties(lp PROPERTIES PUBLIC_HEADER ${lp_HDRS})

target_include_directories(lp PUBLIC include)
if(ENABLE_CLOG)
	target_link_libraries(lp clog)
endif()
target_link_libraries(lp Node)
