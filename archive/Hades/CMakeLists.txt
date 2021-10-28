cmake_minimum_required (VERSION 2.8)
project (hades)

find_package(OpenCV REQUIRED)
find_package(FLTK REQUIRED)
find_package(Threads REQUIRED)

link_directories(
  ${FLTK_LIBRARY_DIRS}
)

include_directories(
  ${FLTK_INCLUDE_DIRS}
  ${OpenCV_INCLUDE_DIRS}
)

#set(CMAKE_BUILD_TYPE Release)
set(CMAKE_BUILD_TYPE Debug)

##version number
set (hades_VERSION_MAJOR 0)
set (hades_VERSION_MINOR 4)

#build options
if(${CMAKE_BUILD_TYPE} STREQUAL "Release")
option(ENABLE_LOG
	"Enable debugging" OFF)
else()
	option(ENABLE_LOG "Enable debugging" ON)
endif()
if(ENABLE_LOG)
	add_definitions(-DENABLE_LOG)
endif()


#miscellaneous
set (BUFFER_SIZE 256)
set (DEFAULT_PORT 12300)

#configure file to add CMake settings to it
configure_file (
	"${PROJECT_SOURCE_DIR}/config.h.in"
	"${PROJECT_SOURCE_DIR}/config.h"
	)


if( WIN32 ) #WINDOWS
    if( ${CMAKE_BUILD_TYPE} STREQUAL "Release")
	    set( CMAKE_CXX_FLAGS "-Wl,-s -Wno-write-strings -Ofast -std=gnu++11 -DWIN32 -mwindows")
    else()
	    set( CMAKE_CXX_FLAGS "-Wno-write-strings -Wall -g -DDEBUG -std=gnu++11 -DWIN32 -mwindows")
    endif()
else()     #LINUX
    if( ${CMAKE_BUILD_TYPE} STREQUAL "Release")
	    set( CMAKE_CXX_FLAGS "-Wl,-s -lsqlite3 -Wno-write-strings -fPIC -Wall -lrt -Ofast -march=native -mtune=native -std=c++11")
    else()
	    set( CMAKE_CXX_FLAGS " -Wno-write-strings -lsqlite3 -lrt -Wall -ggdb3 -DDEBUG -std=c++11")
    endif()
endif()

ADD_SUBDIRECTORY(Node ${CMAKE_CURRENT_SOURCE_DIR}/Node)
ADD_SUBDIRECTORY(clog ${CMAKE_CURRENT_SOURCE_DIR}/clog)

#include_directories ("${PROJECT_BINARY_DIR}")
ADD_EXECUTABLE(hades hades.cpp ${FLTK_INCLUDE_DIRS})
ADD_EXECUTABLE(hadesd hadesd.cpp ${OpenCV_INCLUDE_DIRS})


TARGET_LINK_LIBRARIES(hades ${FLTK_LIBRARIES} ${CMAKE_THREAD_LIBS_INIT} -ldl)
TARGET_LINK_LIBRARIES(hades clog)
TARGET_LINK_LIBRARIES(hades Node)

TARGET_LINK_LIBRARIES(hadesd ${OpenCV_LIBRARIES} ${CMAKE_THREAD_LIBS_INIT} -ldl -lsqlite3)
TARGET_LINK_LIBRARIES(hades clog)
TARGET_LINK_LIBRARIES(hades Node)
