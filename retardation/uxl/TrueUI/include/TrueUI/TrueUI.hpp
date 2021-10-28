#pragma once

#include <TrueUI/temp.hpp>
#include <functional>
#include <vector>

using TrueUIFnCallback = std::function<Iui*(Iin)>;

class TrueUI {
private:
  std::vector<TrueUIFnCallback> fns;

public:
  TrueUI();
  ~TrueUI();

  void add(TrueUIFnCallback fn);
  int run();
};
