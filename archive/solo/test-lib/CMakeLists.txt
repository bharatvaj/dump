cmake_minimum_required(VERSION 3.8)
project(test-lib)

find_package(clog)
add_library(test-lib src/lol.cpp)
add_subdirectory(deps/EventManager)
add_dependencies(test-lib EventManager)
target_include_directories(test-lib PUBLIC clog)
target_link_libraries(test-lib clog)

target_include_directories(test-lib PUBLIC
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
    $<INSTALL_INTERFACE:include>)
target_include_directories(test-lib PUBLIC EventManager)
target_link_libraries(test-lib EventManager)
