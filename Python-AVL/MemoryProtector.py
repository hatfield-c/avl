import time
import psutil

watchProcesses = [ 'python.exe', 'Unity.exe' ]
killPercentage = 80

print("\nActivating Memory Protector...")
print("\nMonitoring the following processes:")
print("    " + str(watchProcesses))
print("These processes will be killed if RAM usage exceeds " + str(killPercentage) + "%")

while True:

    if psutil.virtual_memory().percent > killPercentage:
        processes = []
        for proc in psutil.process_iter():
            if proc.name() in watchProcesses:
                print("Memory leak! Terminating program " + str(proc.name()))
                processes.append((proc, proc.memory_percent()))
        sorted(processes, key=lambda x: x[1])[-1][0].kill()

    time.sleep(0.5)