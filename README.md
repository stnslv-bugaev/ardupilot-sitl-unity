# ArduPilot SITL + Unity Integration

This project integrates ArduPilot SITL with Unity to simulate a drone in a custom 3D environment.

The goal is to create a flexible simulation platform for autonomous UAV testing and visualization using Unity, with real-time telemetry and control via MAVLink.
## 🚀 Features

- Runs ArduPilot SITL (Software-In-The-Loop) in a Docker container.
- Receives MAVLink telemetry via UDP 14550 (standard port).
- Visualizes the drone in Unity.
- Custom terrain/map generated in Blender and imported into Unity.
- Prepares the ground for automatic terrain generation in the future.

## 🧱 Structure
```graphql
ardupilot-sitl-unity/
├── Dockerfile           # Builds SITL image with ArduCopter
├── Unity/               # Unity project files
├── blender/             # Blender scene / exported mesh
├── scripts/             # Utility scripts (planned)
└── README.md
```

## 🐳 Running SITL

You can build your own Docker image or use the fork I created for easier SITL setup:
[`ardupilot-sitl-unity`](https://github.com/stnslv-bugaev/ardupilot-sitl-docker/tree/easier-way)

Example command to run SITL and forward MAVLink packets to Unity via UDP port 14550:

    docker run -it --rm --env SITL_UDP_OUTPUT_ADDRESS=udp:host.docker.internal:14550 ardupilot

This runs the ArduPilot SITL simulator and outputs telemetry packets on UDP port 14550 to be consumed by the Unity project.
## 🎮 Unity Integration

The Unity project listens on UDP port 14550 to receive MAVLink packets from the running SITL simulator.

The MAVLink communication is parsed to update the drone’s position, orientation, and status in Unity.

Make sure Unity is running before starting the container, or add retry logic on connection (planned).
## 🌍 Map Generation (Blender)

Currently, terrain is manually modeled in Blender, then exported as a mesh and imported into Unity for visualization.

Planned:

- Procedural or scriptable terrain generation directly from code or satellite data.

- Real-time environment updates based on simulation input.

## 📦 Dependencies

- Docker

- Unity 2022+

- Blender (for manual map design)

- (Optional) MAVProxy or QGroundControl for debugging

## 📅 Roadmap

- Automatic terrain generation from code

- Mission control via Unity UI

- Collision simulation / physics enhancements

- Integration with real drone firmware

- Logging & replay

## 📜 License

MIT (or your preferred license)
## 🤝 Contributing

PRs are welcome! Feel free to open issues or suggestions.
