cmake_minimum_required(VERSION 3.0)
project(ping LANGUAGES CXX VERSION 0.0.1)

# 3rd-party libraries
find_package(Threads REQUIRED)
find_package(clog)
find_package(Dot)

# ping compiler configuration
file(GLOB_RECURSE ping_SRCS "ping.cpp" "src/*.cpp")
add_executable(ping ${ping_SRCS})
target_include_directories(ping PRIVATE ${CMAKE_SOURCE_DIR}/include)
target_compile_features(ping PRIVATE cxx_range_for)

# link libraries
target_link_libraries(ping ${CMAKE_THREAD_LIBS_INIT})
target_link_libraries(ping ${CLOG_LIBS})
target_link_libraries(ping ${DOT_LIBS})
