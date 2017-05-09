#include <iostream>

#using <System.dll>

using namespace System;
using namespace System::IO::Ports;
using namespace std;

void connect(double angle) {
	int baudRate = 9600;
	String^ portName = "COM4";

	SerialPort^ edison;
	edison = gcnew SerialPort(portName, baudRate);

	try {
		edison->Open();
		edison->WriteLine(angle.ToString());

		cout << angle << endl;

		edison->Close();
	}
	catch (IO::IOException^ e) {
		cout << "Port is not ready." << endl;
	}
	catch (ArgumentException^ e) {
		cout << "Incorrect port name." << endl;
	}
}