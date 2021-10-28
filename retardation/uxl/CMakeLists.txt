cmake_minimum_required(VERSION 3.10.2)

set(CMAKE_CXX_STANDARD 11)

add_executable(baby baby_main.cpp)

add_subdirectory(TrueUI)

target_link_libraries(baby PRIVATE TrueUI)