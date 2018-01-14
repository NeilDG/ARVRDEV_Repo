using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoundaryListener {
	void OnExitBoundary(APoolable poolableObject);
}
