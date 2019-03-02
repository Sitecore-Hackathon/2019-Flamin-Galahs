# Documentation

## Summary

**Category:** Universal Tracker and xConnect

This Hackathon submission demonstrates a way in which the Sitecore platform can be used to drive interactions via SMS. It leverages a combination of Universal Tracker, xConnect, Marketing Automation and EXM to drive an interactive conversation with a customer via SMS, using Twilio as the SMS provider. The process touches on various aspects of the Sitecore Experience Platform to achieve this outcome:
- Universal Tracker – to handle events received via Twilio and WebAPI service
- xConnect – to process interactions from UT and update contacts
- Marketing Automation – to move the customer through a plan and communicate with them along the journey
- EXM – to send SMS messages

## Pre-requisites

This submission requires the following modules to be installed or available for Sitecore 9.1:
- Universal Tracker (latest version for 9.1)
A third party SMS gateway - Twilio - was used to provide SMS sending, receiving and integration functions for this submission.

## Installation

This module requires that:
1. Universal Tracker: a custom channel must be configured in the UT configs so that inbound interactions are picked up and processed for that channel.
2. New Sitecore Message Type created (SMS Message)
3. Insert options set on Message types  for SMS Message
4. Events, channel and goals configured
5. Marketing Automation Plan configured

Please note that this is a solution that requires customisations to be operational.

## Configuration

- Install update package for SMS Message type and insert options
- Create new MA Plan

Twilio config
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/" xmlns:set="http://www.sitecore.net/xmlconfig/set/" xmlns:env="http://www.sitecore.net/xmlconfig/env/">
  <sitecore>
    <settings>
      <setting name="FlaminGalahs.Foundation.MA.Twilio.Sid" value="ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" />
      <setting name="FlaminGalahs.Foundation.MA.Twilio.AuthToken" value="your_auth_token" />
      <setting name="FlaminGalahs.Foundation.MA.Twilio.FromNumber" value="+15017122661" />
    </settings>
  </sitecore>
</configuration>
```

## Usage

Customer Journey
1. A customer receives a call to action from an SMS or other campaign, and texts a message to a number managed by Twilio
2. The SMS is pushed to Sitecore via Universal Tracker which updates the contact
3. The contact is then moved into an Automation Plan
4. The Plan initiates a Send Email event which sends a custom SMS to the customer asking them to send the word QUOTE in an SMS in order to receive a discount.
5. The customer receives the new message and replies via SMS with the word QUOTE
6. The SMS is again handled by Universal Tracker
7. The contact is moved into a new phase of the Automation Plan and a new message sent requesting the user to send the word BUY via SMS
8. The Automation Plan and customer dialogue continues until the Plan has been completed or no further communications are received and the user enrolment in the Plan expires.

A technical overview diagram is included below:

![Architectural overview](./IntegrationArchitecture.png?raw=true "Architectural overview")

## Video

[![Submission video](https://img.youtube.com/vi/dnVynTshTiE/0.jpg)](https://www.youtube.com/watch?v=dnVynTshTiE)
