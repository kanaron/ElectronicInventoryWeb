import React from "react";
import { Link } from "react-router-dom";

const InventoryTable: React.FC = () => {
  // Example data
  const inventoryItems = [
    {
      id: 1,
      type: "Resistor",
      name: "res",
      value: "10kΩ",
      quantity: 100,
      package: "0402",
      dateAdded: "2024-10-10",
    },
    {
      id: 2,
      type: "Capacitor",
      value: "100μF",
      quantity: 50,
      dateAdded: "2024-10-12",
    },
  ];

  return (
    <div className="inventory-table-container">
      <div className="table-controls">
        <Link to="/addItem" className="add-item-button">
          Add item
        </Link>
      </div>
      <table className="inventory-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Quantity</th>
            <th>Package</th>
          </tr>
        </thead>
        <tbody>
          {inventoryItems.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.name}</td>
              <td>{item.quantity}</td>
              <td>{item.package}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default InventoryTable;
