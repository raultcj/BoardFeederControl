#include <iostream>
#include <stdio.h>

using namespace std;

void connect(int motorId, int mov) {
  if (motorId > 0 && motorId < 4 && mov >= 0 && mov <= 1) {
    const char *serialPort = "/dev/ttyUSB0";

    FILE *file;
    file = fopen(serialPort, "w");

    int signal = (motorId * 10) + mov;

    fprintf(file, "%d", signal);
    fclose(file);

  } else {
    cout << "Invalid serial connection" << endl;
  }
}