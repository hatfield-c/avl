using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TaskInterface
{
    void Execute(DataBusInterface addressBus, DataBusInterface commandBus);
}
