cmake_minimum_required(VERSION 3.0)
project(right_initiator)

find_package(Threads)
find_package(crosssocket)
find_package(fmt)

add_executable(right_initiator right_initiator.cpp)

target_include_directories(right_initiator PRIVATE ./)
target_include_directories(right_initiator PRIVATE crosssocket)
target_include_directories(right_initiator PRIVATE fmt)

#TODO use find_package, instead of hard linking
target_link_libraries(right_initiator -lbluetooth)
target_link_libraries(right_initiator ${CMAKE_THREAD_LIBS_INIT})
target_link_libraries(right_initiator crosssocket)
target_link_libraries(right_initiator fmt)

