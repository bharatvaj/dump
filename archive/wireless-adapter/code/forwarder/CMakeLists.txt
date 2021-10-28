cmake_minimum_required(VERSION 3.8)
project(forwarder)

set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${CMAKE_CURRENT_SOURCE_DIR}/cmake")

find_package(comm CONFIG REQUIRED)
find_package(clog CONFIG REQUIRED)
find_package(Threads REQUIRED)
find_package(fmt REQUIRED)
find_package(BlueZ REQUIRED)
find_package(crosssocket CONFIG REQUIRED)
find_package(Dot CONFIG REQUIRED)

file(GLOB FORWARDER_HDRS "*.hpp" "include/*.hpp" "include/device/*.hpp" "include/medium/*.hpp" "include/model/*.hpp" "include/network/*.hpp")
file(GLOB FORWARDER_SRCS "forwarder.cpp")

add_subdirectory(deps/xbee_ansic_library)
add_subdirectory(include/network)

add_executable(forwarder ${FORWARDER_SRCS})

#FIXME should be obtained from xbee
target_compile_definitions(forwarder PUBLIC POSIX)

target_include_directories(
  forwarder PRIVATE
  $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
  $<INSTALL_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
)

target_include_directories(
  forwarder PRIVATE
  $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
  $<INSTALL_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
)

target_include_directories(forwarder PRIVATE ${COMM_INCLUDE_LIBS})
target_include_directories(forwarder PRIVATE fmt)
target_include_directories(forwarder PRIVATE xbee)
target_include_directories(forwarder PRIVATE ${BLUEZ_INCLUDE_DIR})

target_link_libraries(forwarder PRIVATE ${CMAKE_THREAD_LIBS_INIT})
target_link_libraries(forwarder PRIVATE clog::clog)
target_link_libraries(forwarder PRIVATE comm::comm)
target_link_libraries(forwarder PRIVATE Dot)
target_link_libraries(forwarder PRIVATE fmt)
target_link_libraries(forwarder PRIVATE ${BLUEZ_LIBRARIES})
target_link_libraries(forwarder PRIVATE /usr/local/lib/libwifi-scan.so)
target_link_libraries(forwarder PRIVATE xbee)


if(CMAKE_BUILD_TYPE MATCHES "^[Rr]elease")
  find_package(Doxygen QUIET)
  if(DOXYGEN_FOUND)
    set(DOXYGEN_GENERATE_HTML YES)
    set(DOXYGEN_GENERATE_MAN YES)
    set(DOXYGEN_HAVE_DOT YES)
    set(DOXYGEN_CALL_GRAPH YES)
    set(DOXYGEN_CALLER_GRAPH YES)
    set(DOXYGEN_EXTRACT_ALL YES)
    set(DOXYGEN_INCLUDED_BY_GRAPH NO)
    set(DOXYGEN_COLLABORATION_GRAPH NO)
    set(DOXYGEN_EXCLUDE_SYMBOLS "std::*")

    set(DOXYGEN_STRIP_FROM_PATH ${CMAKE_CURRENT_SOURCE_DIR}/include)
    set(DOXYGEN_STRIP_FROM_INC_PATH ${CMAKE_CURRENT_BINARY_DIR})
    doxygen_add_docs(
        ip_merge_docs
        ${FORWARDER_SRCS} ${FORWARDER_HDRS}
        USE_MDFILE_AS_MAINPAGE ${CMAKE_CURRENT_SOURCE_DIR}/README.md
        WORKING_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}
        COMMENT "Generate man and html pages"
  )
  else()
    message("Doxygen need to be installed to generate the doxygen documentation")
  endif()
endif()