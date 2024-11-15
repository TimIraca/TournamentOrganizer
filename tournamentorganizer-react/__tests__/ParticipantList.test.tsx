import React from "react";
import { render } from "@testing-library/react";
import { ParticipantList } from "@/components/features/tournaments/ParticipantList";
import "@testing-library/jest-dom";

const mockParticipants = [
  { id: "1", participantName: "Alice", registrationDate: "2023-11-11" },
  { id: "2", participantName: "Bob", registrationDate: "2023-11-12" },
];

describe("ParticipantList", () => {
  it("renders correctly", () => {
    const { container } = render(
      <ParticipantList participants={mockParticipants} maxParticipants={10} />
    );
    expect(container).toMatchSnapshot();
  });
});
