cmake_minimum_required(VERSION 3.8)
project(lol VERSION 0.0.1)

find_package(comm)

add_executable(lol lol.cpp)

target_link_libraries(lol PRIVATE comm::comm)
