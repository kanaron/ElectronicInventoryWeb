import React, { useEffect, useState } from "react";
import {
  Container,
  Header,
  Segment,
  Table,
  Loader,
  Message,
  Icon,
} from "semantic-ui-react";
import agent from "../../app/agent";
import { PurchaseSuggestion } from "../../models/PurchaseSuggestion";

export default function FindAndPurchasePage() {
  const [suggestions, setSuggestions] = useState<PurchaseSuggestion[] | null>(
    null
  );
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    agent.Purchase.getSuggestions()
      .then((data) => {
        setSuggestions(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setError("Failed to load suggestions");
        setLoading(false);
      });
  }, []);

  if (loading)
    return (
      <Loader
        active
        inline="centered"
        content="Loading purchase suggestions..."
      />
    );

  if (error) return <Message negative content={error} />;

  if (!suggestions || suggestions.length === 0)
    return (
      <Container style={{ marginTop: "2em" }}>
        <Message info icon>
          <Icon name="check circle outline" />
          <Message.Content>
            <Message.Header>No missing parts found</Message.Header>
            All BOM items appear to be sufficiently stocked.
          </Message.Content>
        </Message>
      </Container>
    );

  return (
    <Container style={{ marginTop: "2em" }}>
      <Header as="h2" dividing>
        Find & Purchase Suggestions
      </Header>

      {suggestions.map((item, index) => (
        <Segment key={index}>
          <Header as="h4">
            Missing: {item.category}: {item.bomValue} — {item.package} (Need:{" "}
            {item.quantityNeeded})
          </Header>
          {item.suggestions.length === 0 ? (
            <Message
              warning
              content="No matching TME products found for this item."
            />
          ) : (
            <Table celled compact striped>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell>Symbol</Table.HeaderCell>
                  <Table.HeaderCell>Description</Table.HeaderCell>
                  <Table.HeaderCell>Qty to Order</Table.HeaderCell>
                  <Table.HeaderCell>Total Cost</Table.HeaderCell>
                  <Table.HeaderCell>Next Price Break</Table.HeaderCell>
                  <Table.HeaderCell>Link</Table.HeaderCell>
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {item.suggestions.map((s, i) => (
                  <Table.Row key={i}>
                    <Table.Cell>{s.symbol}</Table.Cell>
                    <Table.Cell>{s.description}</Table.Cell>
                    <Table.Cell>{s.quantityToOrder}</Table.Cell>
                    <Table.Cell>
                      {s.totalCost.toFixed(2)} {s.currency}
                    </Table.Cell>
                    <Table.Cell>
                      {s.nextPriceBreakQuantity && s.nextPriceBreakUnitPrice ? (
                        <>
                          {s.nextPriceBreakQuantity} pcs @{" "}
                          {s.nextPriceBreakUnitPrice.toFixed(2)} {s.currency}
                        </>
                      ) : (
                        "-"
                      )}
                    </Table.Cell>
                    <Table.Cell>
                      <a href={s.url} target="_blank" rel="noopener noreferrer">
                        <Icon name="external alternate" /> TME
                      </a>
                    </Table.Cell>
                  </Table.Row>
                ))}
              </Table.Body>
            </Table>
          )}
        </Segment>
      ))}
    </Container>
  );
}
