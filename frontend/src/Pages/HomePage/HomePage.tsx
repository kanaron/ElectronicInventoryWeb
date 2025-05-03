import { NavLink } from "react-router-dom";
import {
  Button,
  Container,
  Header,
  Segment,
  Table,
  Loader,
  Message,
} from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import agent from "../../app/agent";
import { BomItem } from "../../models/BomItem";
import { InventoryItem } from "../../models/InventoryItem";

export default observer(function HomePage() {
  const { userStore } = useStore();

  const [missingParts, setMissingParts] = useState<BomItem[] | null>(null);
  const [lowStockItems, setLowStockItems] = useState<InventoryItem[] | null>(
    null
  );
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const loadDashboardData = async () => {
      try {
        const [missing, lowStock] = await Promise.all([
          agent.Dashboard.getMissingParts(),
          agent.Dashboard.getLowStockItems(),
        ]);
        setMissingParts(missing);
        setLowStockItems(lowStock);
      } catch (err) {
        setError("Failed to load dashboard data");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    if (userStore.isLoggedIn) {
      loadDashboardData();
    }
  }, [userStore.isLoggedIn]);

  if (!userStore.isLoggedIn) {
    return (
      <Segment inverted textAlign="center" className="home-segment">
        <Container>
          <Header as="h1" inverted className="home-header">
            Electronics Inventory
          </Header>
          <Segment vertical className="button-segment">
            <Button
              primary
              size="large"
              className="home-button"
              content="Login"
              as={NavLink}
              to="/login"
            />
            <Button
              negative
              size="large"
              className="home-button"
              content="Register"
              as={NavLink}
              to="/register"
            />
          </Segment>
        </Container>
      </Segment>
    );
  }

  if (loading) return <Loader active content="Loading dashboard..." />;

  if (error) return <Message negative content={error} />;

  return (
    <Container style={{ marginTop: "2em" }}>
      <Header as="h2" dividing>
        Inventory Dashboard
      </Header>

      <Segment>
        <Header as="h3">Missing BOM Items</Header>
        <Table celled>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Value</Table.HeaderCell>
              <Table.HeaderCell>Package</Table.HeaderCell>
              <Table.HeaderCell>Quantity</Table.HeaderCell>
              <Table.HeaderCell>Description</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {missingParts?.map((item) => (
              <Table.Row key={item.id}>
                <Table.Cell>{item.value}</Table.Cell>
                <Table.Cell>{item.package}</Table.Cell>
                <Table.Cell>{item.quantity}</Table.Cell>
                <Table.Cell>{item.description}</Table.Cell>
              </Table.Row>
            ))}
          </Table.Body>
        </Table>
      </Segment>

      <Segment>
        <Header as="h3">Inventory Below Minimum</Header>
        <Table celled>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Symbol</Table.HeaderCell>
              <Table.HeaderCell>Quantity</Table.HeaderCell>
              <Table.HeaderCell>Min Stock Level</Table.HeaderCell>
              <Table.HeaderCell>Category</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {lowStockItems?.map((item) => (
              <Table.Row key={item.id}>
                <Table.Cell>{item.symbol}</Table.Cell>
                <Table.Cell>{item.quantity}</Table.Cell>
                <Table.Cell>{item.minStockLevel}</Table.Cell>
                <Table.Cell>{item.category}</Table.Cell>
              </Table.Row>
            ))}
          </Table.Body>
        </Table>
      </Segment>
    </Container>
  );
});
