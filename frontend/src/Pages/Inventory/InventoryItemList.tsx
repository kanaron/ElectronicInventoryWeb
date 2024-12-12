import { InventoryItem } from "../../models/InventoryItem";
import {
  Button,
  Image,
  Table,
  TableCell,
  TableHeader,
  TableRow,
} from "semantic-ui-react";

interface Props {
  inventoryItems: InventoryItem[];
  onEdit: (id: number) => void;
  onViewDetails: (id: number) => void;
}

export default function InventoryItemList({
  inventoryItems,
  onEdit,
  onViewDetails,
}: Props) {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <Table.HeaderCell content="Photo" />
          <Table.HeaderCell content="Quantity" />
          <Table.HeaderCell content="Type" />
          <Table.HeaderCell content="Value" />
          <Table.HeaderCell content="Package" />
          <Table.HeaderCell content="Location" />
          <Table.HeaderCell content="Actions" />
        </TableRow>
      </TableHeader>

      <Table.Body>
        {inventoryItems.map((item) => (
          <Table.Row key={item.id}>
            <TableCell>
              <Image src={item.photoUrl} size="small" bordered />
            </TableCell>
            <TableCell content={item.quantity} />
            <TableCell content={item.type} />
            <TableCell content={item.value} />
            <TableCell content={item.package} />
            <TableCell content={item.location} />
            <TableCell>
              <div style={{ display: "flex", alignItems: "center" }}>
                <Button
                  primary
                  onClick={() => onEdit(item.id)}
                  content="Edit"
                  size="small"
                />
                <Button
                  secondary
                  onClick={() => onViewDetails(item.id)}
                  content="Details"
                  size="small"
                />
              </div>
            </TableCell>
          </Table.Row>
        ))}
      </Table.Body>
    </Table>
  );
}
