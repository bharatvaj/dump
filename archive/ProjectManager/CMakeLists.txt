cmake_minimum_required(VERSION 3.0)

project(ProjectManager)

find_package(EventManager)
include(cmake/FindGTK3.cmake)

file(GLOB_RECURSE PM_HDRS include/*.hpp)
file(GLOB_RECURSE PM_SRCS src/*.cpp)

add_subdirectory(ProjectManagerCore)

add_executable(ProjectManager project_manager.cpp ${PM_HDRS} ${PM_SRCS})

target_include_directories(ProjectManager PRIVATE include)
target_include_directories(ProjectManager PRIVATE ProjectMangerCore/include)
target_include_directories(ProjectManager PRIVATE ${GTK3_INCLUDE_DIRS})

target_link_libraries(ProjectManager ProjectManagerCore)
target_link_libraries(ProjectManager ${GTK3_LIBRARIES})
