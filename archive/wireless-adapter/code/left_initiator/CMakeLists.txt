cmake_minimum_required(VERSION 3.0)
project(left_initiator)
find_package(crossguid)
find_package(Dot)

add_executable(left_initiator left_initiator.cpp)


target_include_directories(left_initiator PRIVATE ${DOT_INCLUDE_LIBS})
target_include_directories(left_initiator PRIVATE crossguid)

target_link_libraries(left_initiator ${DOT_LIBS})
target_link_libraries(left_initiator crossguid)
